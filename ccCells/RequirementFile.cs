/*
 * 2017年6月4日 11:13:44 郑少宝 项目需求文件的读取
 * 
 * 我不知道为什么喜欢你，但是你就是我不喜欢别人的理由
 */
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ZrTBMCodeForTestItem.ccCells;
using ZrTBMCodeForTestItem.ccEcternal;

namespace ccCells
{
	/// <summary>
	/// 需求文件
	/// </summary>
	public class RequirementFile
	{
		 
		/// <summary>
		/// 目标窗体宽度
		/// </summary>
		private int formWidth = 1000;
		/// <summary>
		/// 行高
		/// </summary>
		private const int rowHeight = 40;
		/// <summary>
		/// 工作簿
		/// </summary>
		private Workbook workbook;
		public RequirementFile(string filePath)
		{
			FileStream fstream = new FileStream(filePath, FileMode.Open);
			this.workbook = new Workbook(fstream);
		}	
		
		/// <summary>
		/// 获取控件信息
		/// </summary>
		/// <returns></returns>
		public List<ControlDBInfo> GetControlDBInfoForTrust()
		{
			List<ControlDBInfo> list = new List<ControlDBInfo>();
			//连续空行的数量，连续 5 行都为空则退出
			int emptyRowCount = 0;
			for (int x = 1; x < workbook.Worksheets.Count; x++)
			{
				Worksheet sheet = this.workbook.Worksheets[x];
				for (int i = 2; i < 1000; i++)
				{
					if (sheet.Cells[$"A{i}"].Value == null)
					{
						emptyRowCount++;
						if (emptyRowCount >= 5) break;
						continue;
					}
					else
					{
						if (emptyRowCount > 0) emptyRowCount = 0;
						try
						{
							list.Add(
								new ControlDBInfo()
								{
									TableName = sheet.Cells[$"A{i}"].Value.ToString(),
									Description = sheet.Cells[$"B{i}"].Value.ToString(),
									ColumnName = sheet.Cells[$"C{i}"].Value.ToString(),
									TypeLength = sheet.Cells[$"D{i}"].Value.ToString(),
									Default = sheet.Cells[$"E{i}"].Value.ObjToString(),
									Format = sheet.Cells[$"F{i}"].Value.ObjToString(),
									IsEnum = sheet.Cells[$"G{i}"].Value.ToString().ToBool(),
									IsNotNull = sheet.Cells[$"H{i}"].Value.ToString().ToBool(),
									IsReadOnly = sheet.Cells[$"I{i}"].Value.ToString().ToBool(),
									Verify = sheet.Cells[$"J{i}"].Value.ObjToString()
								}
							);
						}
						catch (Exception ex)
						{
							ZrTBMCodeForTestItem.ccCommonFunctions.Function.MsgError($"发生异常，行号：{i},异常信息:{ex.Message}");
							throw ex;
						}
					}
				}
			}			
			return list;
		}

		public void ccControl(Panel pan)
		{
			System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
			for (int sheetIndex = 3; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
			{
				Worksheet sheet = this.workbook.Worksheets[sheetIndex];
				
				int endX = 15;
				string endY = "N";
				List<double> listSheetWidths = getColumnWidth(sheet, out double sheetWidthsSum);

				//列
				for (int columnIndex = 65; columnIndex <= (int)asciiEncoding.GetBytes(endY)[0]; columnIndex++)
				{
					string letter = asciiEncoding.GetString(new byte[] { (byte)columnIndex });
					//行
					for (int rowIndex = 1; rowIndex <= endX; rowIndex++)
					{
						var cell = sheet.Cells[$"{letter}{rowIndex}"];
						var cellValue = cell.Value.ObjToString();
						if (string.IsNullOrEmpty(cellValue)) continue;

						if ($"{letter}{rowIndex}" == "C10")
						{
							int xxxx = 1;
							//ZrTBMCodeForTestItem.ccCommonFunctions.Function.MsgInfo($"{letter}{rowIndex}:{cellValue}");
						}
						if ($"{letter}{rowIndex}" == "H14")
						{
							int xxxx = 1;
							//ZrTBMCodeForTestItem.ccCommonFunctions.Function.MsgInfo($"{letter}{rowIndex}:{cellValue}");
						}
						//int pointX = getLeft(listSheetWidths, columnIndex - 65, sheetWidthsSum);
						//int pointY = rowIndex * 40;
						//黑体代表是控件
						if (sheet.Cells[$"{letter}{rowIndex}"].GetStyle().Font.Color.Name == "Black")
						{
							TextBox txb = new TextBox();
							txb.Parent = pan;
							//txb.Text = cellValue;
							//txb.Width = widths[columnIndex-65];
							setLabelLocation(cell, txb, sheetWidthsSum, listSheetWidths);
						}
						else
						{							
							Label lbl = new Label();
							lbl.Parent = pan;
							lbl.Text = cellValue;
							lbl.AutoSize = true;
							setLabelLocation(cell, lbl, sheetWidthsSum, listSheetWidths);
						}

					}
				}
			}
		}

		private void setLabelLocation(Cell cell, Control c, double sheetWidthsSum, List<double> sheetWidths)
		{
			int width = (int)(sheetWidths[cell.Column]);
			int x = this.getLeft(sheetWidths, cell.Column, sheetWidthsSum);
			int y = cell.Row * 40;
			if (cell.GetStyle().HorizontalAlignment == TextAlignmentType.Center)
			{
				int marginLeft = (width - c.Width) / 2;
				x += marginLeft;
			}
			else if (cell.GetStyle().HorizontalAlignment == TextAlignmentType.Right)
			{
				int marginLeft = width - c.Width;
				x += marginLeft;
			}

			if (cell.GetStyle().VerticalAlignment == TextAlignmentType.Center)
			{
				int marginTop = (rowHeight - c.Height) / 2;
				y += marginTop;
			}
			else if (cell.GetStyle().VerticalAlignment == TextAlignmentType.Bottom)
			{
				int marginTop = rowHeight - c.Height;
				y += marginTop;
			}
			c.Location = new Point(x, y);
		}

		/// <summary>
		/// 得到左坐标
		/// </summary>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private int getLeft(List<double> list, int index, double sheetWidthsSum)
		{
			double result = 0;
			for (int i = 0; i < list.Count; i++)
			{
				result += list[i];
				if (i + 1 > index) break;
			}
			return (int)(result * formWidth / sheetWidthsSum);
		}

		/// <summary>
		/// 获取列宽，用于计算位置
		/// </summary>
		/// <param name="sheet"></param>
		/// <returns></returns>
		private List<double> getColumnWidth(Worksheet sheet, out double sheetWidthsSum)
		{
			var result = new List<double>();
			sheetWidthsSum = 0;
			for (int i = 0; i <= sheet.Cells.MaxColumn; i++)
			{
				//result.Add((int)(sheet.Cells.Columns[i].Width*8.35));
				result.Add(sheet.Cells.Columns[i].Width); // 单位是镑，根据这个，计算总宽度，在根据目标宽度来计算占比，因为这个跟 dpi 有关
				sheetWidthsSum += sheet.Cells.Columns[i].Width;
			}
			return result;
		}
	}
}
