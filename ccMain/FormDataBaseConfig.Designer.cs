namespace ZrTBMCodeForTestItem.ccMain
{
	partial class FormDataBaseConfig
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataBaseConfig));
			this.panel1 = new System.Windows.Forms.Panel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbClose = new System.Windows.Forms.ToolStripButton();
			this.tsbSave = new System.Windows.Forms.ToolStripButton();
			this.tsbTestLink = new System.Windows.Forms.ToolStripButton();
			this.label6 = new System.Windows.Forms.Label();
			this.txbPort = new System.Windows.Forms.TextBox();
			this.txbUserPWD = new System.Windows.Forms.TextBox();
			this.txbUserName = new System.Windows.Forms.TextBox();
			this.txbDataBaseName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txbHost = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.toolStrip1);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.txbPort);
			this.panel1.Controls.Add(this.txbUserPWD);
			this.panel1.Controls.Add(this.txbUserName);
			this.panel1.Controls.Add(this.txbDataBaseName);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.txbHost);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(943, 515);
			this.panel1.TabIndex = 2;
			// 
			// toolStrip1
			// 
			this.toolStrip1.AutoSize = false;
			this.toolStrip1.BackColor = System.Drawing.Color.White;
			this.toolStrip1.Font = new System.Drawing.Font("新宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(10);
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tsbSave,
            this.tsbTestLink});
			this.toolStrip1.Location = new System.Drawing.Point(0, 46);
			this.toolStrip1.Margin = new System.Windows.Forms.Padding(10);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(941, 55);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsbClose
			// 
			this.tsbClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tsbClose.AutoSize = false;
			this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbClose.Image = global::ZrTBMCodeForTestItem.ccMain.Properties.Resources.cancel_48;
			this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbClose.Name = "tsbClose";
			this.tsbClose.Size = new System.Drawing.Size(60, 52);
			this.tsbClose.Text = "关闭";
			this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
			// 
			// tsbSave
			// 
			this.tsbSave.AutoSize = false;
			this.tsbSave.Font = new System.Drawing.Font("新宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.tsbSave.ForeColor = System.Drawing.SystemColors.MenuHighlight;
			this.tsbSave.Image = global::ZrTBMCodeForTestItem.ccMain.Properties.Resources.Save_48;
			this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbSave.Name = "tsbSave";
			this.tsbSave.Size = new System.Drawing.Size(120, 52);
			this.tsbSave.Text = "保存";
			this.tsbSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
			// 
			// tsbTestLink
			// 
			this.tsbTestLink.Font = new System.Drawing.Font("新宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.tsbTestLink.ForeColor = System.Drawing.SystemColors.MenuHighlight;
			this.tsbTestLink.Image = global::ZrTBMCodeForTestItem.ccMain.Properties.Resources.Link_48;
			this.tsbTestLink.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbTestLink.Name = "tsbTestLink";
			this.tsbTestLink.Size = new System.Drawing.Size(128, 52);
			this.tsbTestLink.Text = "测试连接";
			this.tsbTestLink.Click += new System.EventHandler(this.tsbTestLink_Click);
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.SystemColors.MenuHighlight;
			this.label6.Dock = System.Windows.Forms.DockStyle.Top;
			this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label6.ForeColor = System.Drawing.Color.White;
			this.label6.Location = new System.Drawing.Point(0, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(941, 46);
			this.label6.TabIndex = 13;
			this.label6.Text = "数据库连接配置";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txbPort
			// 
			this.txbPort.BackColor = System.Drawing.SystemColors.Info;
			this.txbPort.Location = new System.Drawing.Point(377, 358);
			this.txbPort.Name = "txbPort";
			this.txbPort.ReadOnly = true;
			this.txbPort.Size = new System.Drawing.Size(258, 26);
			this.txbPort.TabIndex = 12;
			this.txbPort.Text = "1433";
			// 
			// txbUserPWD
			// 
			this.txbUserPWD.Location = new System.Drawing.Point(377, 300);
			this.txbUserPWD.Name = "txbUserPWD";
			this.txbUserPWD.Size = new System.Drawing.Size(258, 26);
			this.txbUserPWD.TabIndex = 11;
			// 
			// txbUserName
			// 
			this.txbUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txbUserName.Location = new System.Drawing.Point(377, 242);
			this.txbUserName.Name = "txbUserName";
			this.txbUserName.Size = new System.Drawing.Size(258, 26);
			this.txbUserName.TabIndex = 10;
			// 
			// txbDataBaseName
			// 
			this.txbDataBaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txbDataBaseName.Location = new System.Drawing.Point(377, 184);
			this.txbDataBaseName.Name = "txbDataBaseName";
			this.txbDataBaseName.Size = new System.Drawing.Size(258, 26);
			this.txbDataBaseName.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(332, 363);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 16);
			this.label5.TabIndex = 8;
			this.label5.Text = "端口";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(332, 305);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "密码";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(332, 247);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "用户";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(332, 189);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "库名";
			// 
			// txbHost
			// 
			this.txbHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txbHost.Location = new System.Drawing.Point(377, 126);
			this.txbHost.Name = "txbHost";
			this.txbHost.Size = new System.Drawing.Size(258, 26);
			this.txbHost.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(332, 131);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "地址";
			// 
			// FormDataBaseConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(943, 515);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "FormDataBaseConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormDataBaseConfig";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbClose;
		private System.Windows.Forms.ToolStripButton tsbSave;
		private System.Windows.Forms.ToolStripButton tsbTestLink;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txbHost;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txbPort;
		private System.Windows.Forms.TextBox txbUserPWD;
		private System.Windows.Forms.TextBox txbUserName;
		private System.Windows.Forms.TextBox txbDataBaseName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
	}
}