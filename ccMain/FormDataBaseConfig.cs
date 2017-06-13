/*
 * 2017年6月4日 12:46:53 郑少宝 数据库连接配置
 * 
 * 希望你可以记住我，记住我这样活过，这样在你身边呆过。
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZrTBMCodeForTestItem.ccCommonFunctions;
using ZrTBMCodeForTestItem.ccLanguage;
using ZrTBMCodeForTestItem.ccSystemConfig;

namespace ZrTBMCodeForTestItem.ccMain
{
	public partial class FormDataBaseConfig : Form
	{
		/// <summary>
		/// 加密程序
		/// </summary>
		private	DES des;
		public FormDataBaseConfig()
		{
			InitializeComponent();
			this.ShowInTaskbar = false;
			this.des = new DES();
			this.init();
		}

		private void init()
		{
			UserConfig config = new UserConfig(false);
			//this.txbHost.Text = config.GetConfig("DB_Host");
			//this.txbDataBaseName.Text = config.GetConfig("DB_Name");
			//this.txbUserName.Text = config.GetConfig("DB_User");
			//this.txbUserPWD.Text = config.GetConfig("DB_PWD");
			//this.txbPort.Text = config.GetConfig("DB_port");

			this.txbHost.Text = this.des.Decrypt(config.GetConfig("DB_Host"));
			this.txbDataBaseName.Text = this.des.Decrypt(config.GetConfig("DB_Name"));
			this.txbUserName.Text = this.des.Decrypt(config.GetConfig("DB_User"));
			this.txbUserPWD.Text = this.des.Decrypt(config.GetConfig("DB_PWD"));
			this.txbPort.Text = this.des.Decrypt(config.GetConfig("DB_port"));
		}

		private void tsbClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private string getLinkStr()
		{
			return $@"server={this.txbHost.Text};database={this.txbDataBaseName.Text};uid={this.txbUserName.Text};pwd={this.txbUserPWD.Text}";
		}

		private void tsbSave_Click(object sender, EventArgs e)
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add("DB_Host", this.des.Encrypt(this.txbHost.Text));
			dict.Add("DB_Name", this.des.Encrypt(this.txbDataBaseName.Text));
			dict.Add("DB_User", this.des.Encrypt(this.txbUserName.Text));
			dict.Add("DB_PWD", this.des.Encrypt(this.txbUserPWD.Text));
			dict.Add("DB_port", this.des.Encrypt(this.txbPort.Text));
			UserConfig uc = new UserConfig(true);
			int x = uc.UpdateConfig(dict);
			if (x != dict.Count)
			{
				Function.MsgError(Language.OperateFail);
				return;
			}
			Function.MsgInfo("ok");
		}

		private void tsbTestLink_Click(object sender, EventArgs e)
		{
			#region 读取数据库配置并测试连接
			tsbTestLink.Enabled = false;
			tsbTestLink.Text = Language.BtnTestLinkDoing;
			string linkstr = this.getLinkStr();
			System.Timers.Timer t = new System.Timers.Timer(30);
			t.Enabled = true;
			t.Elapsed += (s, e1) =>
			{
				t.Enabled = false;
				Action callBack = () =>
				{
					tsbTestLink.Enabled = true;
					tsbTestLink.Text = Language.BtnTestLink;
					Function.MsgInfo("ok");
				};
				try
				{
					using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(linkstr))
					{
						conn.Open();
						if (conn.State != System.Data.ConnectionState.Open)
						{
							Function.MsgError(Language.DBLinkFalseTips);
						}
					}
				}
				catch (Exception ex)
				{
					Function.MsgError(ex.Message);
				}
				finally
				{
					this.Invoke(callBack);
				}
			};
			#endregion
		}
	}
}
