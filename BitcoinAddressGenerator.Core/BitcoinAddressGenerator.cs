namespace BitcoinAddressGenerator.Core
{
	using System;
	using System.Collections.Generic;
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

	[PublicAPI]
	public class BitcoinAddressGenerator
	{
		public AddressInfo GenerateAddress(CoinType coinType)
		{
			string privateKeyHex = GenerateKey();
			string privateKeyWif = PrivateKeyHexToWif(privateKeyHex);
			string publicKeyHex = PrivateKeyHexToPublicKeyHex(privateKeyHex);
			string publicKeyHash = PublicKeyHexToPublicKeyHash(publicKeyHex);
			string address = PublicKeyHashToAddress(publicKeyHash, coinType);

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

		public static string GenerateKey()
		{
			ECKeyPairGenerator gen = new ECKeyPairGenerator();
			SecureRandom secureRandom = new SecureRandom();
			X9ECParameters ps = SecNamedCurves.GetByName("secp256k1");
			ECDomainParameters ecParams = new ECDomainParameters(ps.Curve, ps.G, ps.N, ps.H);
			ECKeyGenerationParameters keyGenParam = new ECKeyGenerationParameters(ecParams, secureRandom);
			gen.Init(keyGenParam);

			AsymmetricCipherKeyPair kp = gen.GenerateKeyPair();

			ECPrivateKeyParameters priv = (ECPrivateKeyParameters)kp.Private;

			byte[] hexpriv = priv.D.ToByteArrayUnsigned();
			return ByteArrayToString(hexpriv);
		}

		public static string PrivateKeyHexToWif(string privateHexKey)
		{
			byte[] hex = ValidateAndGetHexPrivateKey(privateHexKey, 0x80);
			if (hex == null) return null;

			return ByteArrayToBase58Check(hex);
		}

		public static string PrivateKeyHexToPublicKeyHex(string privateKeyHex)
		{
			byte[] hex = ValidateAndGetHexPrivateKey(privateKeyHex, 0x00);
			if (hex == null) return null;
			X9ECParameters ps = SecNamedCurves.GetByName("secp256k1");
			BigInteger Db = new BigInteger(hex);
			ECPoint dd = ps.G.Multiply(Db);

			byte[] pubaddr = new byte[65];
			byte[] Y = dd.Y.ToBigInteger().ToByteArray();
			Array.Copy(Y, 0, pubaddr, 64 - Y.Length + 1, Y.Length);
			byte[] X = dd.X.ToBigInteger().ToByteArray();
			Array.Copy(X, 0, pubaddr, 32 - X.Length + 1, X.Length);
			pubaddr[0] = 4;

			return ByteArrayToString(pubaddr);
		}

		public static string PublicKeyHexToPublicKeyHash(string publicKeyHex)
		{
			byte[] hex = ValidateAndGetHexPublicKey(publicKeyHex);
			if (hex == null) return null;

			SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
			byte[] shaofpubkey = sha256.ComputeHash(hex);

			// https://github.com/darrenstarr/RIPEMD160.net
			RIPEMD160 rip = RIPEMD160.Create();
			byte[] ripofpubkey = rip.ComputeHash(shaofpubkey);

			return ByteArrayToString(ripofpubkey);
		}

		public static string PublicKeyHashToAddress(string publicKeyHash, CoinType coinType)
		{
			byte[] hex = ValidateAndGetHexPublicHash(publicKeyHash);
			if (hex == null) return null;

			byte[] hex2 = new byte[21];
			Array.Copy(hex, 0, hex2, 1, 20);

			int cointype = (int) coinType;
			hex2[0] = (byte)(cointype & 0xff);
			return ByteArrayToBase58Check(hex2);
		}

		public static string AddressToPublicKeyHash(string address)
		{
			byte[] hex = Base58ToByteArray(address);
			if (hex == null || hex.Length != 21)
			{
				int L = address.Length;
				if (L >= 33 && L <= 34)
				{
					Console.Error.WriteLine("Address is not valid.");
				}
				else
				{
					Console.Error.WriteLine("Address is not valid.");
				}
				return null;
			}

			return ByteArrayToString(hex, 1, 20);
		}

		private static string ByteArrayToString(byte[] ba)
		{
			return ByteArrayToString(ba, 0, ba.Length);
		}

		private static string ByteArrayToString(byte[] ba, int offset, int count)
		{
			string rv = "";
			int usedcount = 0;
			for (int i = offset; usedcount < count; i++, usedcount++)
			{
				rv += string.Format("{0:x2}", ba[i]);
			}
			return rv;
		}

		private static string ByteArrayToBase58Check(byte[] ba)
		{

			byte[] bb = new byte[ba.Length + 4];
			Array.Copy(ba, bb, ba.Length);
			SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
			byte[] thehash = sha256.ComputeHash(ba);
			thehash = sha256.ComputeHash(thehash);
			for (int i = 0; i < 4; i++) bb[ba.Length + i] = thehash[i];
			return ByteArrayToBase58(bb);
		}

		private static string ByteArrayToBase58(byte[] ba)
		{
			BigInteger addrremain = new BigInteger(1, ba);

			BigInteger big0 = new BigInteger("0");
			BigInteger big58 = new BigInteger("58");

			string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

			string rv = "";

			while (addrremain.CompareTo(big0) > 0)
			{
				int d = Convert.ToInt32(addrremain.Mod(big58).ToString());
				addrremain = addrremain.Divide(big58);
				rv = b58.Substring(d, 1) + rv;
			}

			// handle leading zeroes
			foreach (byte b in ba)
			{
				if (b != 0) break;
				rv = "1" + rv;

			}
			return rv;
		}

		private static byte[] Base58ToByteArray(string base58)
		{
			BigInteger bi2 = new BigInteger("0");
			string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

			bool ignoreChecksum = false;

			foreach (char c in base58)
			{
				if (b58.IndexOf(c) != -1)
				{
					bi2 = bi2.Multiply(new BigInteger("58"));
					bi2 = bi2.Add(new BigInteger(b58.IndexOf(c).ToString()));
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

			byte[] bb = bi2.ToByteArrayUnsigned();

			// interpret leading '1's as leading zero bytes
			foreach (char c in base58)
			{
				if (c != '1') break;
				byte[] bbb = new byte[bb.Length + 1];
				Array.Copy(bb, 0, bbb, 1, bb.Length);
				bb = bbb;
			}

			if (bb.Length < 4) return null;

			if (ignoreChecksum == false)
			{
				SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
				byte[] checksum = sha256.ComputeHash(bb, 0, bb.Length - 4);
				checksum = sha256.ComputeHash(checksum);
				for (int i = 0; i < 4; i++)
				{
					if (checksum[i] != bb[bb.Length - 4 + i]) return null;
				}
			}

			byte[] rv = new byte[bb.Length - 4];
			Array.Copy(bb, 0, rv, 0, bb.Length - 4);
			return rv;
		}

		private static byte[] ValidateAndGetHexPrivateKey(string privateKeyHex, byte leadingbyte)
		{
			byte[] hex = GetHexBytes(privateKeyHex, 32);

			if (hex == null || hex.Length < 32 || hex.Length > 33)
			{
				Console.Error.WriteLine("Hex is not 32 or 33 bytes.");
				return null;
			}

			// if leading 00, change it to 0x80
			if (hex.Length == 33)
			{
				if (hex[0] == 0 || hex[0] == 0x80)
				{
					hex[0] = 0x80;
				}
				else
				{
					Console.Error.WriteLine("Not a valid private key");
					return null;
				}
			}

			// add 0x80 byte if not present
			if (hex.Length == 32)
			{
				byte[] hex2 = new byte[33];
				Array.Copy(hex, 0, hex2, 1, 32);
				hex2[0] = 0x80;
				hex = hex2;
			}

			hex[0] = leadingbyte;
			return hex;

		}

		private static byte[] ValidateAndGetHexPublicKey(string publicKeyHex)
		{
			byte[] hex = GetHexBytes(publicKeyHex, 64);

			if (hex == null || hex.Length < 64 || hex.Length > 65)
			{
				Console.Error.WriteLine("Hex is not 64 or 65 bytes.");
				return null;
			}

			// if leading 00, change it to 0x80
			if (hex.Length == 65)
			{
				if (hex[0] == 0 || hex[0] == 4)
				{
					hex[0] = 4;
				}
				else
				{
					Console.Error.WriteLine("Not a valid public key");
					return null;
				}
			}

			// add 0x80 byte if not present
			if (hex.Length == 64)
			{
				byte[] hex2 = new byte[65];
				Array.Copy(hex, 0, hex2, 1, 64);
				hex2[0] = 4;
				hex = hex2;
			}
			return hex;
		}

		private static byte[] ValidateAndGetHexPublicHash(string publicKeyHash)
		{
			byte[] hex = GetHexBytes(publicKeyHash, 20);

			if (hex == null || hex.Length != 20)
			{
				Console.Error.WriteLine("Hex is not 20 bytes.");
				return null;
			}
			return hex;
		}

		private static byte[] GetHexBytes(string source, int minimum)
		{
			byte[] hex = GetHexBytes(source);
			if (hex == null) return null;
			// assume leading zeroes if we're short a few bytes
			if (hex.Length > (minimum - 6) && hex.Length < minimum)
			{
				byte[] hex2 = new byte[minimum];
				Array.Copy(hex, 0, hex2, minimum - hex.Length, hex.Length);
				hex = hex2;
			}
			// clip off one overhanging leading zero if present
			if (hex.Length == minimum + 1 && hex[0] == 0)
			{
				byte[] hex2 = new byte[minimum];
				Array.Copy(hex, 1, hex2, 0, minimum);
				hex = hex2;

			}

			return hex;
		}

		private static byte[] GetHexBytes(string source)
		{
			List<byte> bytes = new List<byte>();
			// copy s into ss, adding spaces between each byte
			string s = source;
			string ss = "";
			int currentbytelength = 0;
			foreach (char c in s.ToCharArray())
			{
				if (c == ' ')
				{
					currentbytelength = 0;
				}
				else
				{
					currentbytelength++;
					if (currentbytelength == 3)
					{
						currentbytelength = 1;
						ss += ' ';
					}
				}
				ss += c;
			}

			foreach (string b in ss.Split(' '))
			{
				int v = 0;
				if (b.Trim() == "") continue;
				foreach (char c in b.ToCharArray())
				{
					if (c >= '0' && c <= '9')
					{
						v *= 16;
						v += (c - '0');

					}
					else if (c >= 'a' && c <= 'f')
					{
						v *= 16;
						v += (c - 'a' + 10);
					}
					else if (c >= 'A' && c <= 'F')
					{
						v *= 16;
						v += (c - 'A' + 10);
					}

				}
				v &= 0xff;
				bytes.Add((byte)v);
			}

			return bytes.ToArray();
		}
	}
}