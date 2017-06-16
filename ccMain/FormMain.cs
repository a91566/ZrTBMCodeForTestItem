/*
 * 2017年6月2日 09:59:02 郑少宝 导出文件
 * 
 * 也曾想过不顾一切陪你去远方
 * 后来发现你只是说说
 * 我只是想想 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZrTBMCodeForTestItem.ccCells;
using ZrTBMCodeForTestItem.ccCommonFunctions;
using ZrTBMCodeForTestItem.ccEcternal;
using ZrTBMCodeForTestItem.ccLanguage;
using ZrTBMCodeForTestItem.ccSystemConfig;
using ccZrCHKCodeProduction.ccDataBaseProcess;

namespace ZrTBMCodeForTestItem.ccMain
{
	public partial class FormMain : Form
	{
		/// <summary>
		/// 导出代码类
		/// </summary>
		private ExportCodeFile exportCode;
		/// <summary>
		/// 字段对照
		/// </summary>
		private List<ZrControlExternalInfoFromFile> listZrControlInfo;
		/// <summary>
		/// 数据库连接信息
		/// </summary>
		private string dbLinkString;

		public FormMain()
		{
			InitializeComponent();
			this.init();
		}

		/// <summary>
		/// 初始化
		/// </summary>
		private void init()
		{
			this.initEvent();
			this.initTextBox();
			this.initTabControl();
			this.initOtherControlData();
			this.updateVer();
		}

		/// <summary>
		/// 初始化事件
		/// </summary>
		private void initEvent()
		{
			this.txbAssemblyName.TextChanged += (s, e) => this.txbRootNamespace.Text = this.txbAssemblyName.Text;			
			this.txbExitColor.Click += (s, e) =>
			{
				if (this.colorDialog1.ShowDialog() != DialogResult.OK) return;
				this.lblExitColor.BackColor = this.colorDialog1.Color;
				this.txbExitColor.Text = this.toHexColor(this.colorDialog1.Color);
			};
		}

		/// <summary>
		/// 初始化文本框控件
		/// </summary>
		private void initTextBox()
		{
			this.setTextBoxReadOnly(this.txbRootNamespace, true);
			this.setTextBoxReadOnly(this.txbRequirementFile, true);
			this.setTextBoxReadOnly(this.txbDBFile, true);
			this.setTextBoxReadOnly(this.txbTargetFrameworkVersion, true);
			this.setTextBoxReadOnly(this.txbScript, true);
			this.setTextBoxReadOnly(this.txbOutput, true);
			this.setTextBoxReadOnly(this.txbExitColor, true);
			this.txbColumnWidth.SetTextBoxInt();
			this.txbRowHeight.SetTextBoxInt();
			this.txbTargetFrameworkVersion.SetTextBoxFloat();
			this.txbExcelWithToPxScale.SetTextBoxFloat();
			try
			{
				UserConfig config = new UserConfig(false);
				this.txbTargetFrameworkVersion.Text = config.GetConfig(ConfigKey.TargetFrameworkVersion);
				this.txbFolder.Text = config.GetConfig(ConfigKey.Folder.ToString());
				this.txbExcelWithToPxScale.Text = config.GetConfig(ConfigKey.ExcelWithToPxScale);
				this.txbExitColor.Text = config.GetConfig(ConfigKey.ExitColor);
				this.txbColumnWidth.Text = config.GetConfig(ConfigKey.ColumnWidth);
				this.txbRowHeight.Text = config.GetConfig(ConfigKey.RowHeight);
				this.dbLinkString = config.GetDBLinkString();
				this.columnWidthModal(config.GetConfig(ConfigKey.IsFixedColumnWidth).ToBool());
				this.lblExitColor.BackColor = ColorTranslator.FromHtml("#"+this.txbExitColor.Text);
			}
			catch (Exception ex)
			{
				Function.MsgError(ex.Message);
			}
		}

		/// <summary>
		/// 初始化 TabControl 控件
		/// </summary>
		private void initTabControl()
		{
			this.Shown += (s, e) => this.tcMain.ItemSize = new Size((this.tcMain.Width - 20) / 6, 40);
		}

		/// <summary>
		/// 初始化其他内容
		/// </summary>
		private void initOtherControlData()
		{
			this.tsbClose.Visible = false;
			this.toolStripButton1.Visible = false;
			this.txbOutput.Text = $"主程序版本:{Application.ProductVersion.ToString()}{Environment.NewLine}";
		}

		/// <summary>
		/// 检测新版本
		/// </summary>
		private async void updateVer()
		{
			string localVersion = Application.ProductVersion.ToString();
			try
			{
				bool isNew = await Task<bool>.Run(() =>
				{
					var ver = new CodeProductionVersion(this.dbLinkString);
					var serverVersion = ver.GetVersion();
					if (serverVersion == null)
					{
						ver.Update(localVersion, true);
					}
					else
					{
						serverVersion = ver.GetVersion().ToString();
					}
					if (serverVersion != null && localVersion != serverVersion.ToString())
					{
						Version server = new Version(serverVersion.ToString());
						Version local = new Version(localVersion);
						if (local < server)
						{
							output($"有新版本：{serverVersion.ToString()}");
							return true;
						}
						else
						{
							ver.Update(localVersion, false);
						}
					}
					return false;
				});
				this.tsbNewVersion.Visible = isNew;

				//bool isNew = false;
				//await Task<bool>.Run(() =>
				//{
				//	var ver = new CodeProductionVersion(this.dbLinkString);
				//	var serverVersion = ver.GetVersion();
				//	if (serverVersion == null)
				//	{
				//		ver.Update(localVersion, true);
				//	}
				//	else
				//	{
				//		serverVersion = ver.GetVersion().ToString();
				//	}
				//	if (serverVersion != null && localVersion != serverVersion.ToString())
				//	{
				//		Version server = new Version(serverVersion.ToString());
				//		Version local = new Version(localVersion);
				//		if (local < server)
				//		{
				//			output($"有新版本：{serverVersion.ToString()}");
				//			isNew = true;
				//		}
				//		else
				//		{
				//			ver.Update(localVersion, false);
				//		}
				//	}
				//});
				//this.tsbNewVersion.Visible = isNew;
			}
			catch (Exception ex)
			{
				output($"异常：{ex.Message}");
			}
		}
			

		/// <summary>
		/// 列宽模式，处理相应的控件显示状态
		/// </summary>
		/// <param name="isFixedColumnWidth">列宽模式</param>
		private void columnWidthModal(bool isFixedColumnWidth)
		{
			this.txbRowHeight.Visible = isFixedColumnWidth;
			this.txbColumnWidth.Visible = isFixedColumnWidth;
			this.lblColumnWidth.Visible = isFixedColumnWidth;
			this.lblRowHeight.Visible = isFixedColumnWidth;
			this.lblExcelWithToPxScale.Visible = !isFixedColumnWidth;
			this.txbExcelWithToPxScale.Visible = !isFixedColumnWidth;
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			
		}

		/// <summary>
		/// 选择导出文件夹
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnFolder_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.Description = Language.OutFolder;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.txbFolder.Text = dialog.SelectedPath;
			}
		}

		/// <summary>
		/// 设置控件的只读
		/// </summary>
		/// <param name="txb"></param>
		/// <param name="only"></param>
		private void setTextBoxReadOnly(TextBox txb, bool only)
		{
			txb.ReadOnly = only;
			txb.BackColor = only ? SystemColors.Info : Color.White;
		}

	
		/// <summary>
		/// 选择需求文件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsbRequirementFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "请多选文件, 设计文件以及数据库字段对照文件";
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
			this.loadDBRequirementFile(this.txbDBFile.Text, true);
			this.btnDataRefresh.Enabled = true;
			this.btnTrustRefresh.Enabled = true;
			this.btnTrialRefresh.Enabled = true;
			this.btnExSQL.Enabled = true;
			this.btnExportSQL.Enabled = true;
			this.tsbExportProject.Enabled = true;
		}

		/// <summary>
		/// 验证字段是否存在重复
		/// </summary>
		/// <returns></returns>
		private string isDBColoumRepetition()
		{
			if (this.listZrControlInfo == null || this.listZrControlInfo.Count == 0) return null;
			var data = this.listZrControlInfo.Distinct(new ZrControlExternalInfoFromFile()).ToList();
			if (data.Count == this.listZrControlInfo.Count) return null;

			List<ZrControlExternalInfoFromFile> result = this.listZrControlInfo.Except(data).ToList<ZrControlExternalInfoFromFile>();
			List<string> list = new List<string>();
			foreach (var item in result)
			{
				list.Add($"工作簿：{item.SheetName}，行号：{item.RowIndex}，表名：{ item.ZrTable }，字段名：{item.ZrField }，存在重复，其描述为：{item.ZrDescription}。");
				//2017年6月14日 16:49:20 郑少宝 只要字段重复就是重复
				var temp = this.listZrControlInfo.Where(i => i.ZrField == item.ZrField && !string.IsNullOrEmpty(i.ZrField) && i.RowIndex != item.RowIndex);
				foreach (var item2 in temp)
				{
					list.Add($"工作簿：{item2.SheetName}，行号：{item2.RowIndex}，表名：{ item2.ZrTable }，字段名：{item2.ZrField }，存在重复，其描述为：{item2.ZrDescription}。");
				}
				list.Add("");
			}
			return string.Join(Environment.NewLine, list);
		}

		/// <summary>
		/// 载入数据库字段对照内容到内存
		/// </summary>
		/// <param name="filePath">文件绝对路径</param>
		private void loadDBRequirementFile(string filePath, bool must)
		{
			if (string.IsNullOrEmpty(filePath) || System.IO.File.Exists(filePath) == false) Function.MsgError(Language.NoRequirementFile);
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
			this.initDictZrControlInfo(filePath, must);
		}

		/// <summary>
		/// 初始化数据库字段对照(线程出来)
		/// </summary>
		/// <param name="filePath">文件路径</param>
		private async void initDictZrControlInfo(string filePath, bool must)
		{
			if (must || this.listZrControlInfo == null || this.listZrControlInfo.Count == 0)
			{
				var load = new Loading(this.Handle);
				string repResult = null;
				await Task.Run(() =>
				{
					using (var rf = new DBRequirementFile(filePath))
					{
						var data = new List<ZrControlExternalInfoFromFile>();
						this.listZrControlInfo = new List<ZrControlExternalInfoFromFile>();
						rf.GetControlDBInfoFromFile(ref data);
						this.listZrControlInfo = data.Clone() as List<ZrControlExternalInfoFromFile>;
						repResult = this.isDBColoumRepetition();
						if (!string.IsNullOrEmpty(repResult))
						{
							Function.MsgError(Language.RepetitionColumn);
							output(repResult);
						}
						load.HideLoading();
					}
				});
			}			
		}

		/// <summary>
		/// 日志输出
		/// </summary>
		/// <param name="text"></param>
		private void output(string text)
		{
			if (string.IsNullOrEmpty(text)) return;
			//防止线程里调用
			Action a = () =>
			{
				this.txbOutput.Text += Environment.NewLine + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				this.txbOutput.Text += Environment.NewLine + text;
			};
			this.Invoke(a);
		}

		/// <summary>
		/// 选项卡切换
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.tcMain.SelectedTab.Name == "tpUseRemark" && this.webBrowser1.Tag == null)
			{
				this.webBrowser1.Navigate($@"file:\\{Application.StartupPath}\UseRemark.html");
				//this.webBrowser1.Navigate("http://ie.icoa.cn/");
				this.webBrowser1.Tag = true;
			}

		}

		/// <summary>
		/// 验证导出文件夹是否存在
		/// </summary>
		/// <returns></returns>
		private bool existsOutFolder()
		{
			if (!System.IO.Directory.Exists($@"{Application.StartupPath}\masterplate"))
			{
				Function.MsgError($"{Language.NoExistsMasterplate}");
				return false;
			}
			if (!System.IO.Directory.Exists(this.txbFolder.Text))
			{
				Function.MsgError($"{this.txbFolder.Text} {Language.NoExistsFolderForOut}");
				return false;
			}
			string caFilePath = $@"{this.txbFolder.Text}\{this.txbAssemblyName.Text}";
			if (System.IO.Directory.Exists(caFilePath))
			{
				if (Function.MsgOK("目标文件夹已存在，是否删除？"))
				{
					try
					{
						System.IO.Directory.Delete(caFilePath, true);
					}
					catch (Exception ex)
					{
						Function.MsgError(ex.Message);
					}
				}
			}
			return true;
		}



		/// <summary>
		/// 导出项目类库
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsbExportProject_Click(object sender, EventArgs e)
		{
			if (!this.existsOutFolder()) return;
			if (string.IsNullOrEmpty(this.txbZrCode.Text) || string.IsNullOrEmpty(this.txbAssemblyName.Text))
			{
				Function.MsgError(Language.ZrCodeOrNameIsNull);
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
			string scriptFile = $@"{this.txbFolder.Text}\{this.txbAssemblyName.Text}\Script\Script.sql";
			List<string> list = new List<string>()
			{
				$"中润编码:{ this.txbZrCode.Text}",
				$"{Environment.NewLine}",
				$"解决方案名称：{this.txbAssemblyName.Text}",
				$"{Environment.NewLine}",
				$"{Language.OutFolderAsk}{this.txbFolder.Text}"
			};
			if (!Function.MsgOK(string.Join(Environment.NewLine, list))) return;
			var load = new Loading(this.Handle);
			this.exportCode = new ExportCodeFile(this.txbZrCode.Text, this.txbAssemblyName.Text, this.txbRootNamespace.Text, this.txbFolder.Text);
			this.exportCode.ShowOperateInfoEvent += (text) => this.tslOperateInfo.Text = text;
			this.exportCode.InitFolder();
			this.exportCode.Export_csproj();
			this.exportCode.Export_CheckClassControl();
			this.exportCode.Export_AssemblyInfo();
			this.exportCode.Export_VSS();
			this.exportCode.Export_Trust(this.panTrust);
			this.exportCode.Export_Trial(this.panTrial);
			var ctispig = new CreateTable();
			ctispig.ListZrControlExternalInfoFromFile = this.listZrControlInfo;
			var scriptResult = ctispig.Export(scriptFile);
			if (!scriptResult.result)
			{
				Function.MsgError(scriptResult.errorInfo);
			}
			load.HideLoading();
			if (Function.MsgOK(Language.ExportDone))
			{
				this.openExplorerAndSelect($@"{ this.txbFolder.Text }\{this.txbAssemblyName.Text}\{this.txbAssemblyName.Text}.csproj");
			}
		}

		/// <summary>
		/// 打开资源管理器并且选中
		/// </summary>
		private void openExplorerAndSelect(string filePath)
		{
			System.Diagnostics.Process open = new System.Diagnostics.Process();
			open.StartInfo.FileName = "explorer";
			open.StartInfo.Arguments = @"/select," + filePath;
			open.Start();
		}

		private void btnDataRefresh_Click(object sender, EventArgs e)
		{			
			var loading = new Loading(this.Handle);
			this.initListView();
			addColumnsInfo(true);
			this.listView1.Visible = true;
			this.listView1.Dock = DockStyle.Fill;
			this.txbScript.Visible = false;
			this.txbScript.Dock = DockStyle.None;
			loading.HideLoading();
		}

		private void tsbClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnDBConfigSet_Click(object sender, EventArgs e)
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
		/// <param name="clear">是否清空原来的数据</param>
		private void addColumnsInfo(bool clear)
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
				this.listView1.Columns.Add(null, "正则验证", 120, HorizontalAlignment.Center, 7);
			}
			List<ListViewGroup> listGroup = new List<ListViewGroup>();
			foreach (var item in this.listZrControlInfo)
			{
				bool exists = false;
				foreach (var g in listGroup)
				{
					if (g.Header == item.SheetName)
					{
						exists = true;
						break;
					}
				}
				if (!exists)
				{
					ListViewGroup group = new ListViewGroup();
					group.Header = item.SheetName;
					group.HeaderAlignment = HorizontalAlignment.Left;
					listGroup.Add(group);
				}
			}
			int index = 0;
			foreach (var group in listGroup)
			{
				var data = this.listZrControlInfo.Where(i => i.SheetName == group.Header);
				foreach (var item in data)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = $"{index + 1}";
					lvi.SubItems.Add(item.ZrTable);
					lvi.SubItems.Add(item.ZrDescription);
					lvi.SubItems.Add(item.ZrField);
					lvi.SubItems.Add(item.TypeLength);
					lvi.SubItems.Add(item.Default.ToString());
					lvi.SubItems.Add(item.ZrIsNotNull.ToString());
					lvi.SubItems.Add(item.ZrVerify.ToString());
					group.Items.Add(lvi);
					this.listView1.Items.Add(lvi);
					index++;
				}				
				this.listView1.Groups.Add(group);
			}		
		}

		/// <summary>
		/// 初始化需求文件处理类
		/// </summary>
		/// <param name="filePath">文件绝对路径</param>
		private bool fileIsOccupied(string filePath)
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
					return true;
				}
			};
			return false;
		}

		/// <summary>
		/// 预览收样窗体 async
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnTrustRefresh_Click(object sender, EventArgs e)
		{
			if (this.fileIsOccupied(this.txbRequirementFile.Text)) return;
			this.initDictZrControlInfo(this.txbRequirementFile.Text, false);
			if (this.listZrControlInfo == null || this.listZrControlInfo.Count == 0)
			{
				Function.MsgError(Language.NoDBColumnsInfo);
				return;
			}
			showTrust(this.txbRequirementFile.Text);			
		}

		private void showTrust(string filePath)
		{
			var load = new Loading(this.Handle);
			using (var rf = new FormRequirementFile(filePath))
			{
				rf.ccControl(this.panTrust, this.listZrControlInfo.Clone() as List<ZrControlExternalInfoFromFile>, true);
			};
			load.HideLoading();
		}


		private void btnTrialRefresh_Click(object sender, EventArgs e)
		{
			if (this.fileIsOccupied(this.txbRequirementFile.Text)) return;
			this.initDictZrControlInfo(this.txbRequirementFile.Text, false);
			if (this.listZrControlInfo == null || this.listZrControlInfo.Count == 0)
			{
				Function.MsgError(Language.NoDBColumnsInfo);
				return;
			}
			showTrial(this.txbRequirementFile.Text);
		}

		private void showTrial(string filePath)
		{
			var load = new Loading(this.Handle);
			using (var rf = new FormRequirementFile(filePath))
			{
				rf.ccControl(this.panTrial, this.listZrControlInfo.Clone() as List<ZrControlExternalInfoFromFile>, false);
			};
			load.HideLoading();
		}

		/// <summary>
		/// 检查默认值
		/// </summary>
		/// <returns></returns>
		private bool checkDefaultValue()
		{
			if (this.txbColumnWidth.Text.ToInt() < 10 || this.txbColumnWidth.Text.ToInt() > 1000)
			{
				Function.MsgError("列宽值设置无效，限定 10 ~ 1000。");
				return false;
			}
			if (this.txbRowHeight.Text.ToInt() < 20 || this.txbRowHeight.Text.ToInt() > 100)
			{
				Function.MsgError("行高值设置无效，限定 20 ~ 100。");
				return false;
			}
			return true;
		}

		private void tsbSaveDefault_Click(object sender, EventArgs e)
		{
			if (!checkDefaultValue()) return;
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
			dict.Add(ConfigKey.RowHeight.ToString(), this.txbRowHeight.Text);
			dict.Add(ConfigKey.ColumnWidth.ToString(), this.txbColumnWidth.Text);
			dict.Add(ConfigKey.ExitColor.ToString(), this.txbExitColor.Text);
			return dict;
		}

		/// <summary>
		/// 验证转换，Excel 可以识别的颜色名称
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		private string toHexColor(Color color)
		{
			return string.Format("{0}{1}{2}",
				Convert.ToString(color.R, 16).PadLeft(2, '0'),
				Convert.ToString(color.G, 16).PadLeft(2, '0'),
				Convert.ToString(color.B, 16).PadLeft(2, '0'));
		}

		/// <summary>
		/// 异步执行脚本
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnExSQL_Click(object sender, EventArgs e)
		{
			if (!Function.MsgOK(Language.AskExSql)) return;
			var load = new Loading(this.Handle);
			string btnCaption = (sender as Button).Text;
			this.btnExSQL.Enabled = false;
			this.btnExSQL.Text = Language.Doing;
			try
			{
				await Task.Run(() =>
				{
					CreateTable ctispig = new CreateTable();
					ctispig.ListZrControlExternalInfoFromFile = this.listZrControlInfo;
					var result = ctispig.ExSQL(this.dbLinkString);
					if (!result.result)
					{
						Function.MsgError(result.errorInfo);
						return;
					}
					Function.MsgInfo("ok");
				});
			}
			catch (Exception ex)
			{
				Function.MsgError(ex.Message);
			}
			finally
			{
				load.HideLoading();
				this.btnExSQL.Text = btnCaption;
				this.btnExSQL.Enabled = true;
			}
		}


		private async void btnExportSQL_Click(object sender, EventArgs e)
		{
			var load = new Loading(this.Handle);
			string btnCaption = (sender as Button).Text;
			this.btnExportSQL.Enabled = false;
			this.btnExportSQL.Text = Language.Doing;
			try
			{
				(bool result, string errorInfo, List<string> scripts) result = (result:false, errorInfo:null, scripts:null); 
				await Task.Run(() =>
				{
					CreateTable ctispig = new CreateTable();
					ctispig.ListZrControlExternalInfoFromFile = this.listZrControlInfo;
					result = ctispig.Export();
					if (!result.result)
					{
						Function.MsgError(result.errorInfo);
						return;
					}
				});
				this.txbScript.Text = string.Join(Environment.NewLine, result.scripts);
			}
			catch (Exception ex)
			{
				Function.MsgError(ex.Message);
			}
			finally
			{
				load.HideLoading();
				this.txbScript.Visible = true;
				this.txbScript.Dock = DockStyle.Fill;
				this.listView1.Visible = false;
				this.listView1.Dock = DockStyle.None;
				this.btnExportSQL.Text = btnCaption;
				this.btnExportSQL.Enabled = true;
			}
		}		
	}

	
}
