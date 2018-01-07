namespace BitcoinAddressGenerator
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.PictureBox pictureBox1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			System.Windows.Forms.Label label4;
			this.buttonGenerateAddress = new System.Windows.Forms.Button();
			this.comboBoxCoinTypeGenerate = new System.Windows.Forms.ComboBox();
			this.textBoxPrivateKeyHex = new System.Windows.Forms.TextBox();
			this.textBoxPrivateKeyWif = new System.Windows.Forms.TextBox();
			this.textBoxPublicKeyHex = new System.Windows.Forms.TextBox();
			this.textBoxPublicKeyHash = new System.Windows.Forms.TextBox();
			this.textBoxAddress = new System.Windows.Forms.TextBox();
			this.labelAddress = new System.Windows.Forms.Label();
			this.buttonBlockExplorer = new System.Windows.Forms.Button();
			this.comboBoxCoinTypeBlockExplorer = new System.Windows.Forms.ComboBox();
			this.buttonCopyAddress = new System.Windows.Forms.Button();
			this.labelStatus = new System.Windows.Forms.Label();
			this.timer = new System.Windows.Forms.Timer(this.components);
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label1.Location = new System.Drawing.Point(12, 69);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(108, 16);
			label1.TabIndex = 3;
			label1.Text = "Private Key (hex)";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label2.Location = new System.Drawing.Point(9, 113);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(102, 16);
			label2.TabIndex = 5;
			label2.Text = "Private Key (wif)";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label3.Location = new System.Drawing.Point(12, 157);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(103, 16);
			label3.TabIndex = 7;
			label3.Text = "Public Key (hex)";
			// 
			// pictureBox1
			// 
			pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			pictureBox1.Location = new System.Drawing.Point(562, 12);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(50, 50);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label4.Location = new System.Drawing.Point(9, 199);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(111, 16);
			label4.TabIndex = 9;
			label4.Text = "Public Key (hash)";
			// 
			// buttonGenerateAddress
			// 
			this.buttonGenerateAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonGenerateAddress.Location = new System.Drawing.Point(143, 9);
			this.buttonGenerateAddress.Name = "buttonGenerateAddress";
			this.buttonGenerateAddress.Size = new System.Drawing.Size(125, 25);
			this.buttonGenerateAddress.TabIndex = 1;
			this.buttonGenerateAddress.Text = "Generate Address";
			this.buttonGenerateAddress.UseVisualStyleBackColor = true;
			this.buttonGenerateAddress.Click += new System.EventHandler(this.buttonGenerateAddress_Click);
			// 
			// comboBoxCoinTypeGenerate
			// 
			this.comboBoxCoinTypeGenerate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCoinTypeGenerate.Items.AddRange(new object[] {
            "Bitcoin",
            "Testnet"});
			this.comboBoxCoinTypeGenerate.Location = new System.Drawing.Point(12, 12);
			this.comboBoxCoinTypeGenerate.MaxDropDownItems = 2;
			this.comboBoxCoinTypeGenerate.Name = "comboBoxCoinTypeGenerate";
			this.comboBoxCoinTypeGenerate.Size = new System.Drawing.Size(125, 21);
			this.comboBoxCoinTypeGenerate.TabIndex = 2;
			// 
			// textBoxPrivateKeyHex
			// 
			this.textBoxPrivateKeyHex.Location = new System.Drawing.Point(12, 88);
			this.textBoxPrivateKeyHex.Name = "textBoxPrivateKeyHex";
			this.textBoxPrivateKeyHex.ReadOnly = true;
			this.textBoxPrivateKeyHex.Size = new System.Drawing.Size(600, 20);
			this.textBoxPrivateKeyHex.TabIndex = 4;
			// 
			// textBoxPrivateKeyWif
			// 
			this.textBoxPrivateKeyWif.Location = new System.Drawing.Point(12, 132);
			this.textBoxPrivateKeyWif.Name = "textBoxPrivateKeyWif";
			this.textBoxPrivateKeyWif.ReadOnly = true;
			this.textBoxPrivateKeyWif.Size = new System.Drawing.Size(600, 20);
			this.textBoxPrivateKeyWif.TabIndex = 6;
			// 
			// textBoxPublicKeyHex
			// 
			this.textBoxPublicKeyHex.Location = new System.Drawing.Point(12, 176);
			this.textBoxPublicKeyHex.Name = "textBoxPublicKeyHex";
			this.textBoxPublicKeyHex.ReadOnly = true;
			this.textBoxPublicKeyHex.Size = new System.Drawing.Size(600, 20);
			this.textBoxPublicKeyHex.TabIndex = 8;
			// 
			// textBoxPublicKeyHash
			// 
			this.textBoxPublicKeyHash.Location = new System.Drawing.Point(12, 218);
			this.textBoxPublicKeyHash.Name = "textBoxPublicKeyHash";
			this.textBoxPublicKeyHash.ReadOnly = true;
			this.textBoxPublicKeyHash.Size = new System.Drawing.Size(600, 20);
			this.textBoxPublicKeyHash.TabIndex = 10;
			// 
			// textBoxAddress
			// 
			this.textBoxAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxAddress.Location = new System.Drawing.Point(12, 324);
			this.textBoxAddress.Name = "textBoxAddress";
			this.textBoxAddress.ReadOnly = true;
			this.textBoxAddress.Size = new System.Drawing.Size(600, 29);
			this.textBoxAddress.TabIndex = 12;
			// 
			// labelAddress
			// 
			this.labelAddress.AutoSize = true;
			this.labelAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAddress.Location = new System.Drawing.Point(8, 301);
			this.labelAddress.Name = "labelAddress";
			this.labelAddress.Size = new System.Drawing.Size(75, 20);
			this.labelAddress.TabIndex = 11;
			this.labelAddress.Text = "Address";
			// 
			// buttonBlockExplorer
			// 
			this.buttonBlockExplorer.Location = new System.Drawing.Point(487, 359);
			this.buttonBlockExplorer.Name = "buttonBlockExplorer";
			this.buttonBlockExplorer.Size = new System.Drawing.Size(125, 25);
			this.buttonBlockExplorer.TabIndex = 13;
			this.buttonBlockExplorer.Text = "Block Explorer";
			this.buttonBlockExplorer.UseVisualStyleBackColor = true;
			this.buttonBlockExplorer.Click += new System.EventHandler(this.buttonBlockExplorer_Click);
			// 
			// comboBoxCoinTypeBlockExplorer
			// 
			this.comboBoxCoinTypeBlockExplorer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxCoinTypeBlockExplorer.Items.AddRange(new object[] {
            "Bitcoin",
            "Testnet"});
			this.comboBoxCoinTypeBlockExplorer.Location = new System.Drawing.Point(356, 362);
			this.comboBoxCoinTypeBlockExplorer.MaxDropDownItems = 2;
			this.comboBoxCoinTypeBlockExplorer.Name = "comboBoxCoinTypeBlockExplorer";
			this.comboBoxCoinTypeBlockExplorer.Size = new System.Drawing.Size(125, 21);
			this.comboBoxCoinTypeBlockExplorer.TabIndex = 14;
			// 
			// buttonCopyAddress
			// 
			this.buttonCopyAddress.Location = new System.Drawing.Point(487, 390);
			this.buttonCopyAddress.Name = "buttonCopyAddress";
			this.buttonCopyAddress.Size = new System.Drawing.Size(125, 23);
			this.buttonCopyAddress.TabIndex = 15;
			this.buttonCopyAddress.Text = "Copy Address";
			this.buttonCopyAddress.UseVisualStyleBackColor = true;
			this.buttonCopyAddress.Click += new System.EventHandler(this.buttonCopyAddress_Click);
			// 
			// labelStatus
			// 
			this.labelStatus.AutoSize = true;
			this.labelStatus.Location = new System.Drawing.Point(9, 419);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(0, 13);
			this.labelStatus.TabIndex = 16;
			// 
			// timer
			// 
			this.timer.Interval = 5000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// MainForm
			// 
			this.AcceptButton = this.buttonGenerateAddress;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.buttonCopyAddress);
			this.Controls.Add(this.comboBoxCoinTypeBlockExplorer);
			this.Controls.Add(this.buttonBlockExplorer);
			this.Controls.Add(this.textBoxAddress);
			this.Controls.Add(this.labelAddress);
			this.Controls.Add(this.textBoxPublicKeyHash);
			this.Controls.Add(label4);
			this.Controls.Add(this.textBoxPublicKeyHex);
			this.Controls.Add(label3);
			this.Controls.Add(this.textBoxPrivateKeyWif);
			this.Controls.Add(label2);
			this.Controls.Add(this.textBoxPrivateKeyHex);
			this.Controls.Add(label1);
			this.Controls.Add(this.comboBoxCoinTypeGenerate);
			this.Controls.Add(this.buttonGenerateAddress);
			this.Controls.Add(pictureBox1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(640, 480);
			this.MinimumSize = new System.Drawing.Size(640, 480);
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "BitCoin Address Generator";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button buttonGenerateAddress;
		private System.Windows.Forms.ComboBox comboBoxCoinTypeGenerate;
		private System.Windows.Forms.TextBox textBoxPrivateKeyHex;
		private System.Windows.Forms.TextBox textBoxPrivateKeyWif;
		private System.Windows.Forms.TextBox textBoxPublicKeyHex;
		private System.Windows.Forms.TextBox textBoxPublicKeyHash;
		private System.Windows.Forms.TextBox textBoxAddress;
		private System.Windows.Forms.Button buttonBlockExplorer;
		private System.Windows.Forms.Label labelAddress;
		private System.Windows.Forms.ComboBox comboBoxCoinTypeBlockExplorer;
		private System.Windows.Forms.Button buttonCopyAddress;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Timer timer;
	}
}

