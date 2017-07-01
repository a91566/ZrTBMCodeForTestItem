/*
 * 2017年6月4日 11:13:44 郑少宝 项目需求文件的读取
 * 
 * 我不知道为什么喜欢你
 * 但是你就是我不喜欢别人的理由
 */
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZrTBMCodeForTestItem.ccExtend;
using ZrTBMCodeForTestItem.ccSystemConfig;

namespace ZrTBMCodeForTestItem.ccCells
{
	/// <summary>
	/// 需求文件
	/// </summary>
	public class FormRequirementFile : DBRequirementFile, IDisposable
	{
		#region IDisposable
		/// <summary>
		/// 释放标志
		/// </summary>
		bool _disposed;
		/// <summary>
		/// 析构
		/// </summary>
		~FormRequirementFile()
		{
			Dispose(false);
		}
		/// <summary>
		/// 提供子类覆写
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (_disposed) return;
			if (disposing)
			{
				if (this.config != null) this.config = null;
				if (this.workbook != null) this.workbook = null;
				if (this.fstream != null)
				{
					this.fstream.Dispose();
					//this.fstream = null;
				}
				if (this.asciiEncoding != null) this.asciiEncoding = null;
				if (this.currentSheet != null) this.currentSheet = null;
				if (this.currentCell != null) this.currentCell = null;
				if (this.createControl != null) this.createControl = null;
				if (this.listZrControlInfo != null)
				{
					this.listZrControlInfo.Clear();
					this.listZrControlInfo = null;
				}
				if (this.listLabelName != null)
				{
					this.listLabelName.Clear();
					this.listLabelName = null;
				}
			}
			_disposed = true;
		}
		#endregion

		/// <summary>
		/// Excel 列宽 与 像素的比例转换
		/// </summary>
		private readonly double excelWithToPxScale;
		/// <summary>
		/// 列宽（每一列固定的宽度，不读取 Excel 的列宽计算）
		/// </summary>
		private readonly int columnWidth;
		/// <summary>
		/// 是否固定列宽
		/// </summary>
		private readonly bool isFixedColumnWidth;
		/// <summary>
		/// 行高
		/// </summary>
		private readonly int rowHeight;
		/// <summary>
		/// 当前操作的工作簿
		/// </summary>
		private Worksheet currentSheet;
		/// <summary>
		/// 当前读取的单元格
		/// </summary>
		private Cell currentCell;
		/// <summary>
		/// 当前工作簿的总宽度（磅）
		/// </summary>
		private double currentSheetWidth;
		/// <summary>
		/// 当前工作簿每列的宽度（磅）
		/// </summary>
		private List<double> currentSheetColumnWidth;
		/// <summary>
		/// Ascii 码处理
		/// </summary>
		private System.Text.ASCIIEncoding asciiEncoding;
		/// <summary>
		/// 创建控件的操作类
		/// </summary>
		private CreateControl createControl;
		/// <summary>
		/// 字段对照
		/// </summary>
		private List<ZrControlExternalInfoFromFile> listZrControlInfo;
		/// <summary>
		/// Label 控件集合，用于重复的处理
		/// </summary>
		private List<string> listLabelName;
		/// <summary>
		/// 特殊字符串数组
		/// </summary>
		public string[] SpecialChar { get; set; }
		/// <summary>
		/// 需求文件处理类
		/// </summary>
		public FormRequirementFile() : base()
		{
			this.asciiEncoding = new System.Text.ASCIIEncoding();
			this.excelWithToPxScale = Convert.ToDouble(base.config.GetConfig(ConfigKey.ExcelWithToPxScale));
			this.rowHeight = base.config.GetConfig(ConfigKey.RowHeight).ToInt();
			this.columnWidth = base.config.GetConfig(ConfigKey.ColumnWidth).ToInt();
			this.isFixedColumnWidth = base.config.GetConfig(ConfigKey.IsFixedColumnWidth).ToBool();
		}

		/// <summary>
		/// 需求文件处理类
		/// </summary>
		/// <param name="filePath">文件路径</param>
		public FormRequirementFile(string filePath) : this()
		{
			this.fileName = filePath;
			this.asciiEncoding = new System.Text.ASCIIEncoding();
			this.rowHeight = base.config.GetConfig(ConfigKey.RowHeight).ToInt();
			base.InitFile();

		}

		#region 字段操作
		/// <summary>
		/// 获取字段对象信息
		/// </summary>
		/// <param name="columnName">字段名</param>
		/// <returns></returns>
		private ZrControlExternalInfoFromFile getZrControlExternalInfo(string columnName)
		{
			if (this.listZrControlInfo == null || this.listZrControlInfo.Count == 0) return null;
			var a = this.listZrControlInfo.FirstOrDefault(i => i.ZrField == columnName);
			if (a != null)
				return a;
			return null;
		}

		#endregion

		#region 表格控件窗体操作

		/// <summary>
		/// 生成控件，并处理容器的逻辑关系
		/// </summary>
		/// <param name="pan">父容器</param>
		/// <param name="listZrControlInfo">字段对照</param>
		/// <param name="isTrust">是否收样（收样就读一页）</param>
		public void ccControl(Panel pan,List<ZrControlExternalInfoFromFile> listZrControlInfo, bool isTrust)
		{
			pan.Controls.Clear();
			this.listZrControlInfo = listZrControlInfo;
			if (this.createControl == null)
			{
				this.createControl = new CreateControl();
			}
			if (isTrust)
			{
				//label 名称集合初始化
				listLabelName = new List<string>();
				this.currentSheet = this.workbook.Worksheets[1];				
				this.getSheetColumnWidthInfo();
				this.createControlsForCurrentSheet(pan);
			}
			else
			{
				TabControl tc = new TabControl();
				tc.Name = "tcTest";
				tc.Parent = pan;
				tc.Dock = DockStyle.Fill;

				//label 名称集合初始化
				listLabelName = new List<string>();
				for (int sheetIndex = 2; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
				{
					this.currentSheet = this.workbook.Worksheets[sheetIndex];
					TabPage tp = new TabPage();
					tp.AutoScroll = true;
					tp.BackColor = Color.White;
					tc.TabPages.Add(tp);
					tp.Text = this.currentSheet.Name;
					tp.Name = $"tp{tp.Text}";
					this.getSheetColumnWidthInfo();
					createControlsForCurrentSheet(tp);
					tp.MouseWheel += (s, e) => SendKeys.Send(e.Delta.ToString());
				}
			}
			
		}


		/// <summary>
		/// 读取当前页生成控件
		/// </summary>
		/// <param name="parent">容器</param>
		private void createControlsForCurrentSheet(Control parent)
		{		

			Control realParent = null;
			TabControlInfo tcInfo = new TabControlInfo();
			int columnCount = currentSheet.Cells.MaxColumn;
			int tabIndex = 1;

			//行
			for (int rowIndex = 1; rowIndex < 1000; rowIndex++)
			{
				
				//遇到红色背景就退出
				if (this.currentSheet.Cells[$"A{rowIndex}"].GetStyle().ForegroundColor.Name == this.exitColor)
				{
					if (tcInfo != null && tcInfo.TabControl != null)
					{
						this.setTabControlHeightWidth(tcInfo.TabControl);
					}
					Console.WriteLine($"{this.currentSheet.Name},A{rowIndex},Exit");
					return;
				}

				//列
				for (int columnIndex = 0; columnIndex <= columnCount; columnIndex++)
				{
					string letter = asciiEncoding.GetString(new byte[] { (byte)(columnIndex + 65) });
					try
					{
						currentCell = currentSheet.Cells[$"{letter}{rowIndex}"];
						var cellValue = currentCell.Value.ObjToString().Trim();
						if (string.IsNullOrEmpty(cellValue)) continue;
						//如果是分页控件的话，那么在这个分页控件创建完成之前，都不改变 currentCell，
						if (cellValue.ToLower() == "抗折强度判定结果a")
						{
							int aaa = 1;
						}
						if (cellValue.ToLower() == "抗压强度比28d_检测结论")
						{
							int aaa = 1;
						}
						if (cellValue.ToLower() == "tabcontrol")
						{
							var tab = createTabControl();
							tcInfo = tab.tcInfo;
							tcInfo.TabControl.Parent = parent;
							this.setControlLocation(tab.tc, false, new Point(0, 0));
							//跳一行读取数据操作
							rowIndex += 1;
							break;
						}
						if (cellValue.ToLower() == "tabcontrolend")
						{
							tcInfo.IsOperateTabControl = false;
							break;
						}
						if (tcInfo.IsOperateTabControl)
						{
							//过了范围 置为 false 
							if (rowIndex - 1 > tcInfo.ColumnRowIndexEnd[tcInfo.ColumnRowIndexEnd.Count - 1].Y)
							{
								tcInfo.IsOperateTabControl = false;
							}
							else
							{
								int index = getCurrentCellTabPageIndex(tcInfo);
								//不是 tabpage 的背景色
								if (index == -1) continue;
								realParent = tcInfo.TabControl.TabPages[index];
								if (cellValue == realParent.Text) break;
							}
						}

						if (this.getCurrentCellFontColorName() == "Black" || this.getCurrentCellFontColorName() == "0")//黑色字体代表是控件cell.GetMergedRange().ColumnCount
						{
							//生成控件
							ZrControlExternalInfoFromFile dbInfo = this.getZrControlExternalInfo(cellValue);
							Control c = this.createControl.CreateZrControl(dbInfo);
							c.TabIndex = tabIndex;
							tabIndex++;
							//TextBox c = new TextBox();
							//c.ForeColor = Color.Gray;
							//c.TextAlign = HorizontalAlignment.Center;
							//c.Text = $"{letter}{rowIndex}";
							c.Parent = realParent == null ? parent : realParent;
							new ToolTip().SetToolTip(c, $"{letter}{rowIndex}:{cellValue}");
							//第二个参数设置为 false 及不可调整大小
							this.setControlLocation(c, !this.isFixedColumnWidth, tcInfo);
							this.setControlSizeStyle(c);
						}
						else
						{
							Label c = new Label();
							this.setLabelName(c, cellValue);
							c.Parent = realParent == null ? parent : realParent;
							c.Text = cellValue.RemoveNewLine().RemoveEmptyChar();
							c.Font = new System.Drawing.Font(currentCell.GetStyle().Font.Name, currentCell.GetStyle().Font.Size);
							//c.Text = $"{letter}{rowIndex}";
							c.AutoSize = true;
							c.TabIndex = tabIndex;
							tabIndex++;
							setControlLocation(c, false, tcInfo);
						}
					}
					catch (Exception ex)
					{
						ccCommonFunctions.Function.MsgError($"发生异常 CreateControlsForCurrentSheet{Environment.NewLine}" +
							$"文件名：{System.IO.Path.GetFileName(this.fileName)}{Environment.NewLine}" +
							$"工作簿：{this.currentSheet.Name},单元格:{$"{letter}{rowIndex}"}{Environment.NewLine}" +
							$"异常信息:{ex.Message}");
					}
					
				}
			}

		}

		private void setControlSizeStyle(Control c)
		{			
			if (this.isFixedColumnWidth)
			{
				//图片库处理
				if (c is PictureBox)
				{
					(c as PictureBox).BorderStyle = BorderStyle.FixedSingle;
					if (this.currentCell.IsMerged)
					{
						int columns = this.currentCell.GetMergedRange().ColumnCount;
						int rows = this.currentCell.GetMergedRange().RowCount;
						c.Size = new Size(columns * this.columnWidth, this.rowHeight * rows);
					}
				}
				else
				{
					c.Size = new Size(100, 21);
				}
			}
		}

		/// <summary>
		/// 设置 Label 的名称，因为会有可能重复的情况
		/// </summary>
		/// <param name="lbl">文本控件</param>
		/// <param name="cellvalue">文本</param>
		private void setLabelName(Label lbl, string cellvalue)
		{
			string name = $"lbl{cellvalue}".RemoveNewLine().RemoveSpecialChar().RemoveEmptyChar();
			if (name == "lbl试样重量g")
			{
				int x = 1;
			}
			if (this.listLabelName.Contains(name))
			{
				var d = this.listLabelName.Where( i => i.StartsWith(name));
				name += $"_{d.Count()+1}";
			}
			this.listLabelName.Add(name);
			lbl.Name = name;
		}

		/// <summary>
		/// 调整 TabControl 的高度
		/// </summary>
		/// <param name="tc">TabControl 控件</param>
		private void setTabControlHeightWidth(TabControl tc)
		{
			List<int> listTop = new List<int>();
			for (int i = 0; i < tc.TabPages.Count; i++)
			{
				if (tc.TabPages[i].Controls.Count == 0) continue;
				listTop.Add(tc.TabPages[i].Controls[tc.TabPages[i].Controls.Count - 1].Location.Y + this.rowHeight);
			}
			listTop.Sort();
			int max = listTop[listTop.Count - 1];
			//这说明不够大
			if (max > tc.Height - tc.ItemSize.Height)
			{
				tc.Height = max + this.rowHeight + tc.ItemSize.Height;
			}

			List<int> listRight = new List<int>();
			for (int i = 0; i < tc.TabPages.Count; i++)
			{
				foreach (Control item in tc.TabPages[i].Controls)
				{
					listRight.Add(item.Location.X + item.Width);
				}
			}
			listRight.Sort();
			max = listRight[listRight.Count - 1];
			tc.Width = max + listRight[0];
		}

		/// <summary>
		/// 获取当前单元格所在的 tabpage 序号
		/// </summary>
		/// <param name="tcInfo">信息</param>
		/// <returns>序号</returns>
		private int getCurrentCellTabPageIndex(TabControlInfo tcInfo)
		{
			if (tcInfo.ReadPageModal == ReadModal.LeftToRight)
			{
				int x = this.currentCell.Column;
				for (int i = 0; i < tcInfo.ColumnRowIndex.Count; i++)
				{
					if (x <= tcInfo.ColumnRowIndexEnd[i].X)
					{
						return i;
					}
				}
				return -1;
			}
			else
			{
				string color = this.getCurrentCellBackColorName();
				return tcInfo.ListBackgroundColorName.IndexOf(color);
			}
			
		}

		/// <summary>
		/// 读取内嵌页的方向
		/// </summary>
		/// <returns></returns>
		private ReadModal readModal()
		{
			//当前单元格往后读取一格，即为方向，默认从左到右
			string letter = asciiEncoding.GetString(new byte[] { (byte)(this.currentCell.Column + 66) });
			string readmodal = this.currentSheet.Cells[$"{letter}{this.currentCell.Row + 1}"].Value.ObjToString().Trim();
			if (!string.IsNullOrEmpty(readmodal) && readmodal == "↓")
			{
				return ReadModal.TopToBottom;
			}
			return ReadModal.LeftToRight;
		}

		/// <summary>
		/// 读取内嵌页信息
		/// </summary>
		/// <param name="tc"></param>
		/// <param name="tcInfo"></param>
		private void readPagesInfo(TabControl tc, TabControlInfo tcInfo)
		{
			if (tcInfo.ReadPageModal == ReadModal.LeftToRight)
			{
				for (int columnIndex = this.currentCell.Column; columnIndex < currentSheet.Cells.MaxColumn; columnIndex++)
				{
					string letter = asciiEncoding.GetString(new byte[] { (byte)(columnIndex + 65) });
					string tpName = currentSheet.Cells[$"{letter}{this.currentCell.Row + 2}"].Value.ObjToString();
					//读取到一个页
					if (!string.IsNullOrEmpty(tpName))
					{
						TabPage tp = new TabPage();
						tp.BackColor = Color.White;
						tp.Name = $"tp{tpName}";
						tp.Text = tpName;
						tc.TabPages.Add(tp);
						try
						{
							tcInfo.ListBackgroundColorName.Add(currentSheet.Cells[$"{letter}{this.currentCell.Row + 2}"].GetStyle().ForegroundColor.Name);
							tcInfo.ColumnRowIndex.Add(new Point(columnIndex, this.currentCell.Row + 2));
							int mergeColumnCount = currentSheet.Cells[$"{letter}{this.currentCell.Row + 2}"].GetMergedRange().ColumnCount;
							int rowCount = tc.Tag == null ? this.getTabControlRowCount(tc) : (int)tc.Tag;
							tcInfo.ColumnRowCount.Add(new Size(mergeColumnCount, rowCount));
							tcInfo.ColumnRowIndexEnd.Add(new Point(columnIndex + mergeColumnCount, this.currentCell.Row + rowCount + 2));
						}
						catch (Exception ex)
						{
							ccCommonFunctions.Function.MsgError($"发生异常，工作簿：{this.currentSheet.Name},单元格:{letter}{this.currentCell.Row + 2},异常信息:{ex.Message}");
						}
					}
				}
			}
			else//c
			{
				for (int rowIndex = this.currentCell.Row + 1; rowIndex < 999; rowIndex++)
				{
					string letter = asciiEncoding.GetString(new byte[] { (byte)(this.currentCell.Column + 65) });
					if (currentSheet.Cells[$"{letter}{rowIndex}"].Value.ObjToString().ToLower() == "tabcontrolend") break;
					string tempColor = currentSheet.Cells[$"{letter}{rowIndex}"].GetStyle().ForegroundColor.Name;
					if (tempColor != "0" && !tcInfo.ListBackgroundColorName.Contains(tempColor))
					{
						//读取到一个页
						tcInfo.ListBackgroundColorName.Add(tempColor);
						string tpName = currentSheet.Cells[$"{letter}{rowIndex}"].Value.ObjToString();
						TabPage tp = new TabPage();
						tp.BackColor = Color.White;
						tp.Name = $"tp{tpName}";
						tp.Text = tpName;
						tc.TabPages.Add(tp);
						try
						{
							tcInfo.ColumnRowIndex.Add(new Point(this.currentCell.Column, rowIndex));
							int mergeColumnCount = currentSheet.Cells[$"{letter}{this.currentCell.Row + 2}"].GetMergedRange().ColumnCount;
							int rowCount = tc.Tag == null ? this.getTabControlRowCount(tc) : (int)tc.Tag;
							tcInfo.ColumnRowCount.Add(new Size(mergeColumnCount, rowCount));
							tcInfo.ColumnRowIndexEnd.Add(new Point(this.currentCell.Column + mergeColumnCount - 1, rowIndex + rowCount - 1));
						}
						catch (Exception ex)
						{
							ccCommonFunctions.Function.MsgError($"发生异常 ReadPagesInfo，工作簿：{this.currentSheet.Name},单元格:{letter}{this.currentCell.Row + 2},异常信息:{ex.Message}");
						}
					}		
				}
			}
		}

		/// <summary>
		/// 试验处理窗体生成一个内嵌的 选项卡
		/// 内嵌页的下一行必须只能有 tabpage 名称，有几个就创建几个
		/// </summary>
		/// <returns></returns>
		private (TabControl tc, TabControlInfo tcInfo) createTabControl()
		{
			TabControlInfo tcInfo = new TabControlInfo();
			//读取方向
			tcInfo.ReadPageModal = this.readModal();
			tcInfo.ListBackgroundColorName = new List<string>();
			tcInfo.ColumnRowIndex = new List<Point>();
			tcInfo.ColumnRowCount = new List<Size>();
			tcInfo.ColumnRowIndexEnd = new List<Point>();
			tcInfo.IsOperateTabControl = true;

			TabControl tc = new TabControl();
			tc.ItemSize = new Size(100, 30);
			tc.Name = $"tc{this.currentSheet.Name}".RemoveNewLine().RemoveSpecialChar().RemoveSpecialChar();
			//默认为 0 ，为 0 时计算，否则不计算
			tc.Height = 0;
			//TabControl 宽度取获取下一行的宽度（合并）的最大一个
			//string letter = asciiEncoding.GetString(new byte[] { (byte)(this.currentCell.Column + 65) });
			//tc.Width = this.getCellWidth(currentSheet.Cells[$"{letter}{this.currentCell.Row + 2}"]) * 2;

			this.readPagesInfo(tc, tcInfo); 
			tcInfo.TabControl = tc;
			tc.Width = this.getCellWidth(0, tcInfo.ColumnRowCount[0].Width);
			return (tc : tc, tcInfo : tcInfo);
		}		

		/// <summary>
		/// 获取选项卡占几行，算出高度，并且设置
		/// </summary>
		/// <returns>占用行数</returns>
		private int getTabControlRowCount(TabControl tc)
		{
			//先取出标志 tabpage 的底色，用来判断截至
			string letter = this.asciiEncoding.GetString(new byte[] { (byte)(this.currentCell.Column + 65) });
			string color = this.currentSheet.Cells[$"{letter}{this.currentCell.Row + 3}"].GetStyle().ForegroundColor.Name;
			//不知道几行，设置一个较大的数读取，不应该超过一百行
			for (int i = this.currentCell.Row + 3; i < 100; i++)
			{
				//当遇到不一样的颜色之后就退出，并记录其所占用的行数，以及设置行高
				if (this.currentSheet.Cells[$"{letter}{i}"].GetStyle().ForegroundColor.Name != color)
				{
					//占用行数存到 Tag 属性
					tc.Tag = (i - this.currentCell.Row - 3);
					return (i - this.currentCell.Row - 3);
				}
			}
			return 1;
		}

		/// <summary>
		/// 设置控件位置
		/// </summary>
		/// <param name="c"></param>
		/// <param name="isCanSetWidth">是否可以调整宽度</param>
		private void setControlLocation(Control c, bool isCanSetWidth, TabControlInfo tcInfo)
		{
			int x = 0;
			int y = 0;
			//如果是内嵌页的控件
			if (tcInfo != null && tcInfo.IsOperateTabControl)
			{
				TabPage tp = c.Parent as TabPage;
				TabControl tc = c.Parent.Parent as TabControl;
				int tabpageIndex = tc.TabPages.IndexOf(tp);
				if (tcInfo.ReadPageModal == ReadModal.LeftToRight)
				{
					int tabpageMarginLeft = getTabPageMarginLeft(tabpageIndex, tcInfo);
					x = this.getCellWidth(0, this.currentCell.Column);
					x -= tabpageMarginLeft;
					y = this.rowHeight * (this.currentCell.Row - tcInfo.ColumnRowIndex[0].Y + 1);
					if (y >= c.Parent.Height)
					{
						c.Parent.Parent.Height = y + this.rowHeight + (c.Parent.Parent as TabControl).ItemSize.Height;
					}
					if (x + c.Width >= c.Parent.Parent.Width)
					{
						c.Parent.Parent.Width = x + c.Width;
					}
				}
				else
				{
					x = this.getCellWidth(0, this.currentCell.Column);
					y = this.currentCell.Row * this.rowHeight - this.getTabPageMarginTop(tabpageIndex, tcInfo);
				}
			}
			this.setControlLocation(c, isCanSetWidth, new Point(x, y));
		}

		/// <summary>
		/// 设置控件位置
		/// </summary>
		/// <param name="c">控件</param>
		/// <param name="isCanSetWidth">是否可以调整宽度</param>
		private void setControlLocation(Control c, bool isCanSetWidth, Point location)
		{
			var leftWidthRowcount = this.getCurrentCellLeftWidth();
			//进行宽度换算成像素
			int columnWidth = leftWidthRowcount.width;

			int x = location.X;
			int y = location.Y;
			if (x == 0 && y == 0)
			{
				x = leftWidthRowcount.left;
				y = this.currentCell.Row * this.rowHeight;
			}

			//合并多行的属性 
			if (leftWidthRowcount.rowCount > 1)
			{
				if (c is TextBox)
				{
					(c as TextBox).Multiline = true;
					//默认多出 n 行，也为了直观的辨别出是多行
					c.Height = c.Height * leftWidthRowcount.rowCount;
				}
			}
			

			//垂直位置
			int marginTop = 0;
			if (this.currentCell.GetStyle().VerticalAlignment == TextAlignmentType.Center)
			{
				marginTop = ((rowHeight * leftWidthRowcount.rowCount) - c.Height) / 2;
			}
			else if (this.currentCell.GetStyle().VerticalAlignment == TextAlignmentType.Bottom)
			{
				marginTop = (rowHeight * leftWidthRowcount.rowCount) - c.Height;
			}
			if (marginTop > 0)
			{
				y += marginTop;
			}
			//如果宽度比控件小
			if ((columnWidth < c.Width) && isCanSetWidth)
			{
				c.Width = columnWidth - 4;
				x += 2;
			}
			else
			{
				//水平偏移
				int marginLeft = 0;
				if (this.currentCell.GetStyle().HorizontalAlignment == TextAlignmentType.Center)
				{
					marginLeft = (columnWidth - c.Width) / 2;
				}
				else if (this.currentCell.GetStyle().HorizontalAlignment == TextAlignmentType.Right)
				{
					marginLeft = columnWidth - c.Width;
					//如果是右对齐，就处理为右对齐
					if (c is Label)
					{
						(c as Label).TextAlign = ContentAlignment.MiddleRight;
					}
				}
				if (marginLeft > 0)
				{
					x += marginLeft;
				}
			}
			c.Location = new Point(x, y);
		}

		/// <summary>
		/// 获取选项卡距离文档左边的距离
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private int getTabPageMarginLeft(int index, TabControlInfo tcInfo)
		{
			int endColumn = tcInfo.ColumnRowIndex[index].X;
			return this.getCellWidth(0, endColumn);
		}

		/// <summary>
		/// 获取选项卡距离文档顶部的距离
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private int getTabPageMarginTop(int index, TabControlInfo tcInfo)
		{
			//int endRow = tcInfo.ColumnRowIndexEnd[index].Y;
			//return (endRow - tcInfo.ColumnRowIndex[0].Y) * this.rowHeight;
			return (tcInfo.ColumnRowIndex[index].Y - 1) * this.rowHeight;
		}

		/// <summary>
		/// 获取单元格的起始位置与宽度与高度（所占行数），单位是像素
		/// </summary>
		/// <returns></returns>
		private (int left, int width, int rowCount) getCurrentCellLeftWidth()
		{
			int rowCount = 1;
			int columnCount = 1;
			//起始列号
			int start = this.currentCell.Column;
			if (this.currentCell.IsMerged)
			{
				//合并的列数
				columnCount = this.currentCell.GetMergedRange().ColumnCount;
				rowCount = this.currentCell.GetMergedRange().RowCount;
			}

			return this.getCurrentCellLeftWidth(start, columnCount, rowCount);
		}

		/// <summary>
		/// 获取单元格的起始位置与宽度与高度（所占行数），单位是像素
		/// </summary>
		/// <returns></returns>
		private (int left, int width, int rowCount) getCurrentCellLeftWidth(int start, int columnCount, int rowCount)
		{
			int left = this.getCellWidth(0, this.currentCell.Column);
			int width = this.getCellWidth(start, start + columnCount);
			return (left: left, width: width, rowCount: rowCount);
		}

		/// <summary>
		/// 获取单元格列宽，包括合并的，单位是像素
		/// </summary>
		/// <param name="cell">单元格</param>
		/// <returns></returns>
		private int getCellWidth(Cell cell)
		{
			int columnSpan = cell.Column;
			if (cell.IsMerged)
			{
				columnSpan += cell.GetMergedRange().ColumnCount;
			}
			return this.getCellWidth(cell.Column, columnSpan);
		}

		/// <summary>
		/// 获取单元格列宽，包括合并的，单位是像素 0 开始
		/// </summary>
		/// <param name="start">起始列号</param>
		/// <param name="end">结束列号</param>
		/// <returns></returns>
		private int getCellWidth(int start,int end)
		{
			if (this.isFixedColumnWidth)
			{
				return (end - start) * this.columnWidth;
			}
			else
			{
				double cellWidth = 0;
				for (int i = start; i < end; i++)
				{
					cellWidth += this.currentSheetColumnWidth[i];
				}
				return this.banToPX(cellWidth);
			}			
		}				

		/// <summary>
		/// 当前页的磅转像素
		/// </summary>
		/// <param name="ban">磅大小</param>
		/// <returns></returns>
		private int banToPX(double ban)
		{
			//return (int)(ban * formWidth / this.currentSheetWidth);
			return (int)(ban * this.excelWithToPxScale);
		}

		/// <summary>
		/// 获取列宽，用于当前页的列宽总宽度信息
		/// </summary>
		private void getSheetColumnWidthInfo()
		{
			//如果是固定列宽就不需要读取了
			if (this.isFixedColumnWidth) return;
			this.currentSheetColumnWidth = new List<double>();
			this.currentSheetWidth = 0;
			for (int i = 0; i <= this.currentSheet.Cells.MaxColumn; i++)
			{
				this.currentSheetColumnWidth.Add(this.currentSheet.Cells.Columns[i].Width); // 单位是镑，根据这个，计算总宽度，在根据目标宽度来计算占比，因为这个跟 dpi 有关
				this.currentSheetWidth += this.currentSheet.Cells.Columns[i].Width;
			}
		}

		/// <summary>
		/// 获取当前单元格的背景色
		/// </summary>
		/// <returns></returns>
		private string getCurrentCellBackColorName()
		{
			if(this.currentCell == null) return null;
			return this.currentCell.GetStyle().ForegroundColor.Name;
		}

		/// <summary>
		/// 获取当前单元格的字体色
		/// </summary>
		/// <returns></returns>
		private string getCurrentCellFontColorName()
		{
			if (this.currentCell == null) return null;
			return this.currentCell.GetStyle().Font.Color.Name;
		}
		
		#endregion
	}
}
