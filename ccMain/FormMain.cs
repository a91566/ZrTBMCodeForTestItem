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

namespace ZrTBMCodeForTestItem.ccMain
{
	public partial class FormMain : Form
	{
		/// <summary>
		/// 导出代码类
		/// </summary>
		private ExportCodeFile exportCode;
		/// <summary>
		/// 需求文件操作类
		/// </summary>
		private RequirementFile requirementFile;
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
			this.txbTableHeaderRowIndex.Text = config.GetConfig("TableHeaderRowIndex");
			this.txbTargetFrameworkVersion.Text = config.GetConfig("TargetFrameworkVersion");
			this.txbFolder.Text = config.GetConfig("Folder");
			this.setTextBoxReadOnly(this.txbRootNamespace, true); 
			this.setTextBoxReadOnly(this.txbRequirementFile, true);
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
			openFileDialog.Filter = Language.RequirementFileFilter;

			if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;
			this.txbRequirementFile.Text = openFileDialog.FileName;
			this.requirementFile = new RequirementFile(openFileDialog.FileName);
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
			if (this.requirementFile == null)
			{
				Function.MsgError(Language.NoRequirementFile);
				return;
			}
			var loading = new Loading();
			this.initListView();
			addColumnsInfo(this.requirementFile.GetColumns(Convert.ToInt16(this.txbTableHeaderRowIndex.Text)), true);
			Function.MsgInfo(this.requirementFile.GetTableName());
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
		private void addColumnsInfo(List<ColumnInfo> list, bool clear)
		{
			if (clear)
			{
				this.listView1.Items.Clear();
				this.listView1.Columns.Clear();
				this.listView1.Columns.Add(null, "序号", 80, HorizontalAlignment.Center, 0);
				this.listView1.Columns.Add(null, "字段名称", 200, HorizontalAlignment.Left, 1);
				this.listView1.Columns.Add(null, "字段类型", 140, HorizontalAlignment.Center, 2);
				this.listView1.Columns.Add(null, "长度", 100, HorizontalAlignment.Center, 3);
				this.listView1.Columns.Add(null, "默认值", 200, HorizontalAlignment.Left, 4);
				this.listView1.Columns.Add(null, "说明", 400, HorizontalAlignment.Left, 5);
			}

			foreach (var item in list)
			{
				ListViewItem lvi = new ListViewItem();
				lvi.Text = item.ID.ToString();
				lvi.SubItems.Add(item.Name);
				lvi.SubItems.Add(item.Type);
				lvi.SubItems.Add(item.Length);
				lvi.SubItems.Add(item.Default);
				lvi.SubItems.Add(item.Remark);
				this.listView1.Items.Add(lvi);
			}
		}

		

		private void btnTrustRefresh_Click(object sender, EventArgs e)
		{
			if (this.requirementFile == null)
			{
				Function.MsgError(Language.NoRequirementFile);
				return;
			}
			var loading = new Loading();
			this.initListView();
			List<ControlInfo> list = this.requirementFile.GetControls(Convert.ToInt16(this.txbTableHeaderRowIndex.Text));
			this.panTrust.Controls.Clear();
			Point startLocation = new Point(10,10);
			foreach (ControlInfo item in list)
			{
				addLabelTextBox(ref startLocation, item);
			}
			loading.HideLoading();
		}

		//(int right, int top) return (txb.Location.X + txb.Width, txb.Top);
		private void addLabelTextBox(ref Point startLocation, ControlInfo item)
		{
			Label lbl = new Label();
			lbl.TextAlign = ContentAlignment.MiddleRight;
			lbl.AutoSize = true;
			lbl.Text = item.Label;
			lbl.Parent = this.panTrust;
			lbl.Location = new Point(startLocation.X + 20, startLocation.Y);
			ZrControl.ZrDynamicTextBox txb = new ZrControl.ZrDynamicTextBox();
			txb.Name = $"txb{item.Label}";
			txb.Parent = this.panTrust;
			txb.Location = new Point(lbl.Location.X + lbl.Width + 6, startLocation.Y - 4);
			startLocation = new Point(txb.Location.X + txb.Width, startLocation.Y);
		}
	}
}
