/*
 * 2017年6月4日 11:13:44 郑少宝 项目需求文件的读取
 * 
 * 我不知道为什么喜欢你，但是你就是我不喜欢别人的理由
 */
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.IO;
using ZrTBMCodeForTestItem.ccCells;

namespace ccCells
{
	/// <summary>
	/// 需求文件
	/// </summary>
	public class RequirementFile
	{
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
		/// 获取表名
		/// </summary>
		/// <returns></returns>
		public string GetTableName()
		{
			return this.workbook.Worksheets[0].Cells["d2"].Value.ToString();
		}

		/// <summary>
		/// 获取字段信息
		/// </summary>
		/// <param name="tableHeaderRowIndex">表头行号</param>
		/// <returns></returns>
		public List<ColumnInfo> GetColumns(int tableHeaderRowIndex)
		{
			Worksheet sheet = this.workbook.Worksheets[0];
			List<ColumnInfo> list = new List<ColumnInfo>();
			for (int i = tableHeaderRowIndex + 1; i < 1000; i++)
			{
				if (sheet.Cells[$"G{i}"].Value == null) break;
				try
				{
					list.Add(
						new ColumnInfo()
						{
							ID = Convert.ToInt16(sheet.Cells[$"A{i}"].Value.ToString()),
							Name = sheet.Cells[$"G{i}"].Value.ToString(),
							Type = sheet.Cells[$"H{i}"].Value.ToString(),
							Length = sheet.Cells[$"I{i}"].Value.ToString(),
							Default = sheet.Cells[$"J{i}"].Value == null ? "" : sheet.Cells[$"J{i}"].Value.ToString(),
							Remark = sheet.Cells[$"F{i}"].Value == null ? "" : sheet.Cells[$"F{i}"].Value.ToString()
						}
					);
				}
				catch (Exception ex)
				{
					ZrTBMCodeForTestItem.ccCommonFunctions.Function.MsgError($"发生异常，行号：{i+1},异常信息:{ex.Message}");
					throw ex;
				}
			}
			return list;
		}

		/// <summary>
		/// 获取控件信息
		/// </summary>
		/// <param name="tableHeaderRowIndex">表头行号</param>
		/// <returns></returns>
		public List<ControlInfo> GetControls(int tableHeaderRowIndex)
		{
			Worksheet sheet = this.workbook.Worksheets[0];
			List<ControlInfo> list = new List<ControlInfo>();
			for (int i = tableHeaderRowIndex + 1; i < 1000; i++)
			{
				if (sheet.Cells[$"G{i}"].Value == null) break;
				try
				{
					list.Add(
						new ControlInfo()
						{
							ID = Convert.ToInt16(sheet.Cells[$"A{i}"].Value.ToString()),
							Classify = sheet.Cells[$"C{i}"].Value.ToString(),
							Label = sheet.Cells[$"D{i}"].Value.ToString(),
							Size = sheet.Cells[$"E{i}"].Value == null ? "" : sheet.Cells[$"E{i}"].Value.ToString(),
							Description = sheet.Cells[$"F{i}"].Value == null ? sheet.Cells[$"D{i}"].Value.ToString() : sheet.Cells[$"F{i}"].Value.ToString()
						}
					);
				}
				catch (Exception ex)
				{
					ZrTBMCodeForTestItem.ccCommonFunctions.Function.MsgError($"发生异常，行号：{i + 1},异常信息:{ex.Message}");
					throw ex;
				}
			}
			return list;
		}
	}
}
