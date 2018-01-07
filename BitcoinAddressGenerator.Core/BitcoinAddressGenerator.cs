namespace BitcoinAddressGenerator.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using JetBrains.Annotations;
	using Org.BouncyCastle.Asn1.Sec;
	using Org.BouncyCastle.Asn1.X9;
	using Org.BouncyCastle.Crypto;
	using Org.BouncyCastle.Crypto.Generators;
	using Org.BouncyCastle.Crypto.Parameters;
	using Org.BouncyCastle.Math;
	using Org.BouncyCastle.Security;
	using ECPoint = Org.BouncyCastle.Math.EC.ECPoint;

	/// <summary>
	/// Credit: https://bitcointalk.org/index.php?topic=25141.0
	/// </summary>
	[PublicAPI]
	public class BitcoinAddressGenerator
	{
		public AddressInfo GenerateAddress(CoinType coinType)
		{
			string privateKeyHex = this.GenerateKey();
			string privateKeyWif = this.PrivateKeyHexToWif(privateKeyHex);
			string publicKeyHex = this.PrivateKeyHexToPublicKeyHex(privateKeyHex);
			string publicKeyHash = this.PublicKeyHexToPublicKeyHash(publicKeyHex);
			string address = this.PublicKeyHashToAddress(publicKeyHash, coinType);

			return new AddressInfo
			{
				PrivateKeyHex = privateKeyHex,
				PrivateKeyWif = privateKeyWif,
				PublicKeyHex = publicKeyHex,
				PublicKeyHash = publicKeyHash,
				Address = address,
				CoinType = coinType,
			};
		}

		private string GenerateKey()
		{
			ECKeyPairGenerator gen = new ECKeyPairGenerator();
			SecureRandom secureRandom = new SecureRandom();
			X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
			ECDomainParameters domainParameters = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
			ECKeyGenerationParameters keyGenerationParameters = new ECKeyGenerationParameters(domainParameters, secureRandom);
			gen.Init(keyGenerationParameters);

			AsymmetricCipherKeyPair keyPair = gen.GenerateKeyPair();

			ECPrivateKeyParameters privateKeyParameters = (ECPrivateKeyParameters)keyPair.Private;

			byte[] privateKeyBytes = privateKeyParameters.D.ToByteArrayUnsigned();
			return this.ByteArrayToString(privateKeyBytes);
		}

		private string PrivateKeyHexToWif(string privateHexKey)
		{
			byte[] privateKeyBytes = this.ValidateAndGetPrivateKeyBytes(privateHexKey, 0x80);
			if (privateKeyBytes == null)
			{
				return null;
			}

			return this.ByteArrayToBase58Check(privateKeyBytes);
		}

		private string PrivateKeyHexToPublicKeyHex(string privateKeyHex)
		{
			byte[] privateKeyBytes = this.ValidateAndGetPrivateKeyBytes(privateKeyHex, 0x00);
			if (privateKeyBytes == null)
			{
				return null;
			}

			X9ECParameters curves = SecNamedCurves.GetByName("secp256k1");
			BigInteger privateKeyInteger = new BigInteger(privateKeyBytes);
			ECPoint p = curves.G.Multiply(privateKeyInteger);

			byte[] publicAddress = new byte[65];
			byte[] y = p.Normalize().YCoord.ToBigInteger().ToByteArray();
			Array.Copy(y, 0, publicAddress, 64 - y.Length + 1, y.Length);
			byte[] x = p.Normalize().XCoord.ToBigInteger().ToByteArray();
			Array.Copy(x, 0, publicAddress, 32 - x.Length + 1, x.Length);
			publicAddress[0] = 4;

			return this.ByteArrayToString(publicAddress);
		}

		private string PublicKeyHexToPublicKeyHash(string publicKeyHex)
		{
			byte[] publicKeyBytes = this.ValidateAndGetPublicKeyBytes(publicKeyHex);
			if (publicKeyBytes == null)
			{
				return null;
			}

			SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
			byte[] publicKeySha = sha256.ComputeHash(publicKeyBytes);

			// Credit: https://github.com/darrenstarr/RIPEMD160.net
			RIPEMD160 rip = RIPEMD160.Create();
			byte[] publicKeyShaRip = rip.ComputeHash(publicKeySha);

			return this.ByteArrayToString(publicKeyShaRip);
		}

		private string PublicKeyHashToAddress(string publicKeyHash, CoinType coinType)
		{
			byte[] publicKeyBytes = this.ValidateAndGetHexPublicHash(publicKeyHash);
			if (publicKeyBytes == null)
			{
				return null;
			}

			byte[] aux = new byte[21];
			Array.Copy(publicKeyBytes, 0, aux, 1, 20);

			aux[0] = (byte)((int)coinType & 0xff);
			return this.ByteArrayToBase58Check(aux);
		}

		private string AddressToPublicKeyHash(string address)
		{
			byte[] base58Bytes = this.Base58ToByteArray(address);
			if (base58Bytes == null || base58Bytes.Length != 21)
			{
				int l = address.Length;
				if (l >= 33 && l <= 34)
				{
					Console.Error.WriteLine("Address is not valid.");
				}
				else
				{
					Console.Error.WriteLine("Address is not valid.");
				}
				return null;
			}

			return this.ByteArrayToString(base58Bytes, 1, 20);
		}

		private string ByteArrayToString(byte[] bytes)
		{
			return this.ByteArrayToString(bytes, 0, bytes.Length);
		}

		private string ByteArrayToString(byte[] bytes, int offset, int count)
		{
			string result = "";

			int usedCount = 0;
			for (int i = offset; usedCount < count; i++, usedCount++)
			{
				result += string.Format("{0:x2}", bytes[i]);
			}

			return result;
		}

		private string ByteArrayToBase58Check(byte[] bytes)
		{
			byte[] aux = new byte[bytes.Length + 4];
			Array.Copy(bytes, aux, bytes.Length);

			SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
			byte[] hashBytes = sha256.ComputeHash(bytes);
			hashBytes = sha256.ComputeHash(hashBytes);

			for (int i = 0; i < 4; i++)
			{
				aux[bytes.Length + i] = hashBytes[i];
			}

			return this.ByteArrayToBase58(aux);
		}

		private string ByteArrayToBase58(byte[] bytes)
		{
			BigInteger addRemainInteger = new BigInteger(1, bytes);

			BigInteger big58 = new BigInteger("58");

			const string allowedChars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

			string result = "";

			while (addRemainInteger.CompareTo(BigInteger.Zero) > 0)
			{
				int d = Convert.ToInt32(addRemainInteger.Mod(big58).ToString());
				addRemainInteger = addRemainInteger.Divide(big58);
				result = allowedChars.Substring(d, 1) + result;
			}

			// Handle leading zeroes.
			foreach (byte b in bytes)
			{
				if (b != 0)
				{
					break;
				}
				result = "1" + result;
			}

			return result;
		}

		private byte[] Base58ToByteArray(string base58)
		{
			BigInteger aux = new BigInteger("0");
			string allowedChars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

			bool ignoreChecksum = false;

			foreach (char c in base58)
			{
				if (allowedChars.IndexOf(c) != -1)
				{
					aux = aux.Multiply(new BigInteger("58"));
					aux = aux.Add(new BigInteger(allowedChars.IndexOf(c).ToString()));
				}
				else if (c == '?')
				{
					ignoreChecksum = true;
				}
				else
				{
					return null;
				}
			}

			byte[] auxBytes = aux.ToByteArrayUnsigned();

			// iInterpret leading '1's as leading zero bytes.
			foreach (char c in base58)
			{
				if (c != '1')
				{
					break;
				}

				byte[] bytes = new byte[auxBytes.Length + 1];
				Array.Copy(auxBytes, 0, bytes, 1, auxBytes.Length);
				auxBytes = bytes;
			}

			if (auxBytes.Length < 4)
			{
				return null;
			}

			if (ignoreChecksum == false)
			{
				SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
				byte[] checksum = sha256.ComputeHash(auxBytes, 0, auxBytes.Length - 4);
				checksum = sha256.ComputeHash(checksum);
				for (int i = 0; i < 4; i++)
				{
					if (checksum[i] != auxBytes[auxBytes.Length - 4 + i])
					{
						return null;
					}
				}
			}

			byte[] result = new byte[auxBytes.Length - 4];
			Array.Copy(auxBytes, 0, result, 0, auxBytes.Length - 4);
			return result;
		}

		private byte[] ValidateAndGetPrivateKeyBytes(string privateKeyHex, byte leadingByte)
		{
			byte[] privateKeyBytes = this.GetHexStringBytes(privateKeyHex, 32);

			if (privateKeyBytes == null || privateKeyBytes.Length < 32 || privateKeyBytes.Length > 33)
			{
				Console.Error.WriteLine("Hex is not 32 or 33 bytes.");
				return null;
			}

			// If leading 00, change it to 0x80.
			if (privateKeyBytes.Length == 33)
			{
				if (privateKeyBytes[0] == 0 || privateKeyBytes[0] == 0x80)
				{
					privateKeyBytes[0] = 0x80;
				}
				else
				{
					Console.Error.WriteLine("Not a valid private key");
					return null;
				}
			}

			// Add 0x80 byte if not present.
			if (privateKeyBytes.Length == 32)
			{
				byte[] bytes = new byte[33];
				Array.Copy(privateKeyBytes, 0, bytes, 1, 32);
				bytes[0] = 0x80;
				privateKeyBytes = bytes;
			}

			privateKeyBytes[0] = leadingByte;
			return privateKeyBytes;

		}

		private byte[] ValidateAndGetPublicKeyBytes(string publicKeyHex)
		{
			byte[] publicKeyBytes = this.GetHexStringBytes(publicKeyHex, 64);

			if (publicKeyBytes == null || publicKeyBytes.Length < 64 || publicKeyBytes.Length > 65)
			{
				Console.Error.WriteLine("Hex is not 64 or 65 bytes.");
				return null;
			}

			// If leading 00, change it to 0x80.
			if (publicKeyBytes.Length == 65)
			{
				if (publicKeyBytes[0] == 0 || publicKeyBytes[0] == 4)
				{
					publicKeyBytes[0] = 4;
				}
				else
				{
					Console.Error.WriteLine("Not a valid public key");
					return null;
				}
			}

			// Add 0x80 byte if not present.
			if (publicKeyBytes.Length == 64)
			{
				byte[] bytes = new byte[65];
				Array.Copy(publicKeyBytes, 0, bytes, 1, 64);
				bytes[0] = 4;
				publicKeyBytes = bytes;
			}

			return publicKeyBytes;
		}

		private byte[] ValidateAndGetHexPublicHash(string publicKeyHash)
		{
			byte[] publicKeyHashBytes = this.GetHexStringBytes(publicKeyHash, 20);

			if (publicKeyHashBytes == null || publicKeyHashBytes.Length != 20)
			{
				Console.Error.WriteLine("Hex is not 20 bytes.");
				return null;
			}

			return publicKeyHashBytes;
		}

		private byte[] GetHexStringBytes(string hexString, int minimum)
		{
			byte[] bytes = this.GetHexStringBytes(hexString);
			if (bytes == null)
			{
				return null;
			}

			// Assume leading zeroes if we're short a few bytes.
			if (bytes.Length > minimum - 6 && bytes.Length < minimum)
			{
				byte[] hex2 = new byte[minimum];
				Array.Copy(bytes, 0, hex2, minimum - bytes.Length, bytes.Length);
				bytes = hex2;
			}
			// Clip off one overhanging leading zero if present.
			if (bytes.Length == minimum + 1 && bytes[0] == 0)
			{
				byte[] auxBytes = new byte[minimum];
				Array.Copy(bytes, 1, auxBytes, 0, minimum);
				bytes = auxBytes;

			}

			return bytes;
		}

		private byte[] GetHexStringBytes(string hexString)
		{
			IList<byte> bytes = new List<byte>();

			// Copy hexString into hexStringWithSpaces, adding spaces between each byte.
			string hexStringWithSpaces = "";
			int currentByteLength = 0;
			foreach (char c in hexString)
			{
				if (c == ' ')
				{
					currentByteLength = 0;
				}
				else
				{
					currentByteLength++;
					if (currentByteLength == 3)
					{
						currentByteLength = 1;
						hexStringWithSpaces += ' ';
					}
				}
				hexStringWithSpaces += c;
			}

			foreach (string str in hexStringWithSpaces.Split(' '))
			{
				int v = 0;
				if (str.Trim() == "")
				{
					continue;
				}

				foreach (char c in str)
				{
					if (c >= '0' && c <= '9')
					{
						v *= 16;
						v += c - '0';

					}
					else if (c >= 'a' && c <= 'f')
					{
						v *= 16;
						v += c - 'a' + 10;
					}
					else if (c >= 'A' && c <= 'F')
					{
						v *= 16;
						v += c - 'A' + 10;
					}

				}
				v &= 0xff;
				bytes.Add((byte)v);
			}

			return bytes.ToArray();
		}
	}
}