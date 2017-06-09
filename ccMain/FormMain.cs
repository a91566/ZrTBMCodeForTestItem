/*
 * 2017年6月2日 09:59:02 郑少宝 导出文件
 * 
 * 也曾想过不顾一切陪你去远方，后来发现你只是说说，我只是想想。
 * 
 */
using ccCells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZrTBMCodeForTestItem.ccCells;
using ZrTBMCodeForTestItem.ccCommonFunctions;
using ZrTBMCodeForTestItem.ccSystemConfig;
using ZrTBMCodeForTestItem.ccEcternal;

namespace ZrTBMCodeForTestItem.ccMain
{
	public partial class FormMain : Form
	{
		/// <summary>
		/// 导出代码类
		/// </summary>
		private ExportCodeFile exportCode;
		/// <summary>
		/// 收样控件列表
		/// </summary>
		private List<ControlDBInfo> listControlDBTrust;
		/// <summary>
		/// 试验控件列表
		/// </summary>
		private List<ControlDBInfo> listControlDBTrial;
		public FormMain()
		{
			InitializeComponent();
			this.init();
		}

		private void init()
		{
			this.initEvent();
			this.initTextBox();
		}

		private void initEvent()
		{
			this.txbAssemblyName.TextChanged += (s, e) => this.txbRootNamespace.Text = this.txbAssemblyName.Text;
		}

		private void initTextBox()
		{
			UserConfig config = new UserConfig(false);
			this.txbTargetFrameworkVersion.Text = config.GetConfig("TargetFrameworkVersion");
			this.txbFolder.Text = config.GetConfig("Folder");
			this.txbMaxWidth.Text = config.GetConfig("MaxWidth");
			this.setTextBoxReadOnly(this.txbRootNamespace, true); 
			this.setTextBoxReadOnly(this.txbRequirementFile, true);
			this.setTextBoxReadOnly(this.txbDBFile, true);
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			
		}

		private void btnFolder_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.Description = Language.OutFolder;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.txbFolder.Text = dialog.SelectedPath;
			}
		}

		private void setTextBoxReadOnly(TextBox txb, bool only)
		{
			txb.ReadOnly = only;
			txb.BackColor = only ? SystemColors.Info : Color.White;
		}

	

		private void tsbRequirementFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "请多选";
			openFileDialog.Multiselect = true;
			openFileDialog.Filter = Language.RequirementFileFilter;

			if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;
			for (int i = 0; i < openFileDialog.FileNames.Length; i++)
			{
				string name = openFileDialog.FileNames.GetValue(i).ToString();
				if (name.Contains("数据库") || name.Contains("字段"))
				{
					this.txbDBFile.Text = name;
				}
				else if (name.Contains("收样") || name.Contains("试验") || name.Contains("窗体"))
				{
					this.txbRequirementFile.Text = name;
				}
			}
			if (string.IsNullOrEmpty(this.txbRequirementFile.Text) || string.IsNullOrEmpty(this.txbDBFile.Text))
			{
				Function.MsgError("文件选择错误.");
				return;
			}
			this.loadRequirementFile(this.txbRequirementFile.Text, this.txbDBFile.Text);
		}

		/// <summary>
		/// 载入去求文件到内存
		/// </summary>
		/// <param name="filePath">文件绝对路径</param>
		/// <param name="filePath">文件绝对路径</param>
		private void loadRequirementFile(string fileRequirementFile, string fileDB)
		{
			var load = new Loading(this.Handle);
			System.Timers.Timer t = new System.Timers.Timer(30);
			t.Interval = 30;
			t.Enabled = true;
			t.Elapsed += (s, e1) =>
			{
				t.Enabled = false;
				Action done = () =>
				{
					load.HideLoading();
				};
				try
				{
					var rf = new RequirementFile(fileDB);
					this.listControlDBTrust = rf.GetControlDBInfoForTrust();
				}
				catch (Exception ex)
				{
					Function.MsgError(ex.Message);
				}
				finally
				{
					this.Invoke(done);
				}
			};
		}

		private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.tcMain.SelectedTab.Name == "tpUseRemark" && this.webBrowser1.Tag == null)
			{
				this.webBrowser1.Navigate($@"file:\\{Application.StartupPath}\UseRemark.html");
				//this.webBrowser1.Navigate("http://ie.icoa.cn/");
				this.webBrowser1.Tag = true;
			}

		}

		private void tsbExportProject_Click(object sender, EventArgs e)
		{
			if (!Function.MsgOK($"{Language.OutFolderAsk}{this.txbFolder.Text}")) return;
			this.exportCode = new ExportCodeFile(this.txbZrCode.Text, this.txbAssemblyName.Text, this.txbRootNamespace.Text, this.txbFolder.Text);
			this.exportCode.InitFolder();
			this.exportCode.Export_csproj();
			this.exportCode.Export_CheckClassControl();
			this.exportCode.Export_AssemblyInfo();
			this.exportCode.Export_Trust();
			this.exportCode.Export_Trial();
			Function.MsgInfo("ok");
		}

		private void btnDataRefresh_Click(object sender, EventArgs e)
		{
			if (this.listControlDBTrust == null)
			{
				Function.MsgError(Language.NoRequirementFile);
				return;
			}
			var loading = new Loading(this.Handle);
			this.initListView();
			addColumnsInfo(this.listControlDBTrust, true);
			//Function.MsgInfo(this.requirementFile.GetTableName());
			loading.HideLoading();
		}

		private void tsbClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			FormDataBaseConfig f = new FormDataBaseConfig();
			f.ShowDialog();
		}

		private void initListView()
		{
			if (this.listView1.Tag != null) return;
			this.listView1.View = View.Details;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			ImageList imageList = new ImageList();
			imageList.ImageSize = new Size(24, 24);
			imageList.Images.Add(Properties.Resources.Number0);
			imageList.Images.Add(Properties.Resources.Number1);
			imageList.Images.Add(Properties.Resources.Number2);
			imageList.Images.Add(Properties.Resources.Number3);
			imageList.Images.Add(Properties.Resources.Number4);
			imageList.Images.Add(Properties.Resources.Number5);
			imageList.Images.Add(Properties.Resources.Number6);
			imageList.Images.Add(Properties.Resources.Number7);
			imageList.Images.Add(Properties.Resources.Number8);
			imageList.Images.Add(Properties.Resources.Number9);
			this.listView1.SmallImageList = imageList;
			this.listView1.Tag = true;
		}

		/// <summary>
		/// 添加数据到 ListView
		/// </summary>
		/// <param name="list">字段信息列表</param>
		/// <param name="clear">是否清空原来的数据</param>
		private void addColumnsInfo(List<ControlDBInfo> list, bool clear)
		{
			if (clear)
			{
				this.listView1.Items.Clear();
				this.listView1.Columns.Clear();
				this.listView1.Columns.Add(null, "序号", 80, HorizontalAlignment.Center, 0);
				this.listView1.Columns.Add(null, "表名", 200, HorizontalAlignment.Left, 1);
				this.listView1.Columns.Add(null, "描述", 400, HorizontalAlignment.Left, 2);
				this.listView1.Columns.Add(null, "字段名称", 200, HorizontalAlignment.Left, 3);
				this.listView1.Columns.Add(null, "字段类型长度", 140, HorizontalAlignment.Center, 4);
				this.listView1.Columns.Add(null, "默认值", 200, HorizontalAlignment.Left, 5);
				this.listView1.Columns.Add(null, "不为空", 100, HorizontalAlignment.Center, 6);
			}
			for (int i = 0; i < list.Count; i++)
			{
				ListViewItem lvi = new ListViewItem();
				lvi.Text = $"{i+1}";
				lvi.SubItems.Add(list[i].TableName);
				lvi.SubItems.Add(list[i].Description);
				lvi.SubItems.Add(list[i].ColumnName);
				lvi.SubItems.Add(list[i].TypeLength);
				lvi.SubItems.Add(list[i].Default.ToString());
				lvi.SubItems.Add(list[i].IsNotNull.ToString());
				this.listView1.Items.Add(lvi);
			}
			
		}


		/// <summary>
		/// 预览收样窗体 async
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnTrustRefresh_Click(object sender, EventArgs e)
		{
			if (this.listControlDBTrust == null)
			{
				Function.MsgError(Language.NoRequirementFile);
				return;
			}
			btnTrustRefresh.Enabled = false;
			var loading = new Loading(this.Handle);
			this.panTrust.Controls.Clear();
			Point startLocation = new Point(10, 10);
			//int x = this.txbRowHeight.Text.ToInt();
			//new CreateControl(this.panTrust, this.txbMaxWidth.Text.ToInt(), this.txbRowHeight.Text.ToInt()).Create(this.listControlDBTrust, startLocation);
			loading.HideLoading();
			btnTrustRefresh.Enabled = true;
		}

		private void button2_Click(object sender, EventArgs e)
		{
		
		}

		private void btnTrialRefresh_Click(object sender, EventArgs e)
		{
			showTrial(this.txbRequirementFile.Text);
		}

		private void showTrial(string filePath)
		{
			var load = new Loading(this.Handle);
			var rf = new RequirementFile(filePath);
			rf.ccControl(this.panTrial);
			load.HideLoading();
		}

		private void button2_Click_1(object sender, EventArgs e)
		{

			System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
			string strCharacter = asciiEncoding.GetString(new byte[] { 65,90 });
			MessageBox.Show(strCharacter);
		}
	}

	
}
