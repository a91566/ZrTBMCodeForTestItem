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
using ZrTBMCodeForTestItem.ccEcternal;

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

		public void ccControl()
		{
			System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
			for (int x = 3; x < workbook.Worksheets.Count; x++)
			{
				Worksheet sheet = this.workbook.Worksheets[x];
				int endX = 15;
				string endY = "N";
				for (int y = 65; y <= (int)asciiEncoding.GetBytes(endY)[0]; y++)
				{
					string letter = asciiEncoding.GetString(new byte[] { (byte)y });
					for (int z = 1; z <= endX; z++)
					{
						var temp = sheet.Cells[$"{letter}{z}"].Value.ObjToString();
						if ($"{letter}{z}" == "D10")
						{
							if (sheet.Cells[$"{letter}{z}"].GetStyle().Font.Color.Name == "Black")
							{

							}
							int xxxx = 1;
							ZrTBMCodeForTestItem.ccCommonFunctions.Function.MsgInfo($"{letter}{z}:{temp}");
						}
					}
				}
			}
		}
	}
}
