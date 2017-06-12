/*
 * 2017年6月2日 09:59:02 郑少宝 导出文件
 * 
 * 也曾想过不顾一切陪你去远方，后来发现你只是说说，我只是想想。
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZrTBMCodeForTestItem.ccCells;
using ZrTBMCodeForTestItem.ccCommonFunctions;
using ZrTBMCodeForTestItem.ccLanguage;
using ZrTBMCodeForTestItem.ccSystemConfig;

namespace ZrTBMCodeForTestItem.ccMain
{
	public partial class FormMain : Form
	{
		/// <summary>
		/// 导出代码类
		/// </summary>
		private ExportCodeFile exportCode;
		/// <summary>
		/// 需求文件读取类
		/// </summary>
		private FormRequirementFile requirementFile;
		/// <summary>
		/// 字段对照
		/// </summary>
		private Dictionary<string, List<ZrControlExternalInfoFromFile>>  dictZrControlInfo;

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
			this.txbTargetFrameworkVersion.Text = config.GetConfig(ConfigKey.TargetFrameworkVersion.ToString());
			this.txbFolder.Text = config.GetConfig(ConfigKey.Folder.ToString());
			this.txbExcelWithToPxScale.Text = config.GetConfig(ConfigKey.ExcelWithToPxScale.ToString());
			this.txbExitColor.Text = config.GetConfig(ConfigKey.ExitColor.ToString());
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
			this.loadDBRequirementFile(this.txbDBFile.Text);
			this.initRequirementFile(this.txbRequirementFile.Text);
			this.btnDataRefresh.Enabled = true;
			this.btnTrustRefresh.Enabled = true;
			this.btnTrialRefresh.Enabled = true;
		}

		/// <summary>
		/// 载入数据库字段对照内容到内存
		/// </summary>
		/// <param name="filePath">文件绝对路径</param>
		private void loadDBRequirementFile(string filePath)
		{
			bool occupied = ZsbApps.GetFileStatus.IsFileOccupied(filePath);
			while (occupied)
			{
				if (Function.MsgOK($"文件：{System.IO.Path.GetFileName(filePath)} 被占用，是否重试？"))
				{
					occupied = ZsbApps.GetFileStatus.IsFileOccupied(filePath);
				}
				else
				{
					return;
				}
			}

			var load = new Loading(this.Handle);
			System.Timers.Timer t = new System.Timers.Timer(100);
			t.Elapsed += (s, e1) =>
			{
				(s as System.Timers.Timer).Enabled = false;
				Action done = () =>
				{
					load.HideLoading();
				};
				try
				{
					var rf = new DBRequirementFile(filePath);
					this.dictZrControlInfo = rf.GetControlDBInfoForTrust();
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
			t.Enabled = true;
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
			if (string.IsNullOrEmpty(this.txbDBFile.Text) || string.IsNullOrEmpty(this.txbRequirementFile.Text))
			{
				Function.MsgError(Language.NoRequirementFile);
				return;
			}

			if (this.panTrust.Controls.Count == 0 || this.panTrial.Controls.Count == 0)
			{
				this.btnTrustRefresh_Click(null,null);
				this.btnTrialRefresh_Click(null, null);
			}
			if (this.panTrust.Controls.Count == 0 || this.panTrial.Controls.Count == 0)
			{
				Function.MsgError(Language.RequirementFileNotUse);
				return;
			}

			if (!Function.MsgOK($"{Language.OutFolderAsk}{this.txbFolder.Text}")) return;
			var load = new Loading(this.Handle);
			this.exportCode = new ExportCodeFile(this.txbZrCode.Text, this.txbAssemblyName.Text, this.txbRootNamespace.Text, this.txbFolder.Text);
			this.exportCode.ShowOperateInfoEvent += (text) => this.tslOperateInfo.Text = text;
			this.exportCode.InitFolder();
			this.exportCode.Export_csproj();
			this.exportCode.Export_CheckClassControl();
			this.exportCode.Export_AssemblyInfo();
			this.exportCode.Export_Trust(this.panTrust);
			this.exportCode.Export_Trial(this.panTrust);
			load.HideLoading();
			if (Function.MsgOK(Language.ExportDone))
			{
				System.Diagnostics.Process open = new System.Diagnostics.Process();
				open.StartInfo.FileName = "explorer";
				open.Start();
			}
		}

		private void btnDataRefresh_Click(object sender, EventArgs e)
		{
			if (this.dictZrControlInfo == null)
			{
				Function.MsgError(Language.NoRequirementFile);
				return;
			}
			var loading = new Loading(this.Handle);
			this.initListView();
			addColumnsInfo(this.dictZrControlInfo, true);
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
		/// <param name="dict">字段信息列表</param>
		/// <param name="clear">是否清空原来的数据</param>
		private void addColumnsInfo(Dictionary<string, List<ZrControlExternalInfoFromFile>> dict, bool clear)
		{
			if (clear)
			{
				this.listView1.Items.Clear();
				this.listView1.Columns.Clear();
				this.listView1.Columns.Add(null, "序号", 80, HorizontalAlignment.Center, 0);
				this.listView1.Columns.Add(null, "表名", 200, HorizontalAlignment.Left, 1);
				this.listView1.Columns.Add(null, "描述", 400, HorizontalAlignment.Left, 2);
				this.listView1.Columns.Add(null, "字段名称", 200, HorizontalAlignment.Left, 3);
				this.listView1.Columns.Add(null, "字段类型长度", 160, HorizontalAlignment.Center, 4);
				this.listView1.Columns.Add(null, "默认值", 120, HorizontalAlignment.Left, 5);
				this.listView1.Columns.Add(null, "不为空", 100, HorizontalAlignment.Center, 6);
			}
			foreach (var item in dict)
			{
				ListViewGroup group = new ListViewGroup();
				group.Header = item.Key;
				group.HeaderAlignment = HorizontalAlignment.Left;   //设置组标题文本的对齐方式。（默认为Left）  
				for (int i = 0; i < item.Value.Count; i++)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = $"{i + 1}";
					lvi.SubItems.Add(item.Value[i].ZrTable);
					lvi.SubItems.Add(item.Value[i].ZrDescription);
					lvi.SubItems.Add(item.Value[i].ZrField);
					lvi.SubItems.Add(item.Value[i].TypeLength);
					lvi.SubItems.Add(item.Value[i].Default.ToString());
					lvi.SubItems.Add(item.Value[i].ZrIsNotNull.ToString());
					group.Items.Add(lvi);
					this.listView1.Items.Add(lvi);
				}
				this.listView1.Groups.Add(group);
			}		
		}

		/// <summary>
		/// 初始化需求文件处理类
		/// </summary>
		/// <param name="filePath">文件绝对路径</param>
		private void initRequirementFile(string filePath)
		{			
			if (this.requirementFile != null) return;
			bool occupied = ZsbApps.GetFileStatus.IsFileOccupied(filePath);
			while (occupied)
			{
				if (Function.MsgOK($"文件：{System.IO.Path.GetFileName(filePath)} 被占用，是否重试？"))
				{
					occupied = ZsbApps.GetFileStatus.IsFileOccupied(filePath);
				}
				else
				{
					return;
				}
			}
			this.requirementFile = new FormRequirementFile(filePath);
		}

		/// <summary>
		/// 预览收样窗体 async
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnTrustRefresh_Click(object sender, EventArgs e)
		{
			this.initRequirementFile(this.txbRequirementFile.Text);
			showTrust(this.txbRequirementFile.Text);			
		}

		private void showTrust(string filePath)
		{
			var load = new Loading(this.Handle);
			this.requirementFile.ccControl(this.panTrust, this.dictZrControlInfo, true);
			load.HideLoading();
		}


		private void btnTrialRefresh_Click(object sender, EventArgs e)
		{
			this.initRequirementFile(this.txbRequirementFile.Text);
			showTrial(this.txbRequirementFile.Text);
		}

		private void showTrial(string filePath)
		{			
			var load = new Loading(this.Handle);
			this.requirementFile.ccControl(this.panTrial, this.dictZrControlInfo, false);
			load.HideLoading();
		}

	

		private void tsbSaveDefault_Click(object sender, EventArgs e)
		{
			UserConfig uc = new UserConfig(true);
			int result = uc.UpdateConfig(this.getDefault());
			if (result > 0)
			{
				Function.MsgInfo("ok");
			}
			else
			{
				Function.MsgError("false");
			}			
		}

		/// <summary>
		/// 获取默认值信息
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, string> getDefault()
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add(ConfigKey.TargetFrameworkVersion.ToString(), this.txbTargetFrameworkVersion.Text);
			dict.Add(ConfigKey.Folder.ToString(), this.txbFolder.Text);
			dict.Add(ConfigKey.ExcelWithToPxScale.ToString(), this.txbExcelWithToPxScale.Text);
			dict.Add(ConfigKey.ExitColor.ToString(), this.txbExitColor.Text);
			return dict;
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			
		}
	}

	
}
