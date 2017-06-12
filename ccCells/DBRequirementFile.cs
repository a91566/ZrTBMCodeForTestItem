/*
 * 2017年6月11日 08:48:01 郑少宝
 * 
 * 我想你一定很忙，所以你只看前三个字就好了。
 */
using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.IO;
using ZrTBMCodeForTestItem.ccEcternal;
using ZrTBMCodeForTestItem.ccSystemConfig;

namespace ZrTBMCodeForTestItem.ccCells
{
	/// <summary>
	/// 数据库字段对照处理
	/// </summary>
	public class DBRequirementFile
	{

		/// <summary>
		/// 用户配置
		/// </summary>
		protected UserConfig config { get; set; }

		/// <summary>
		/// 工作簿
		/// </summary>
		protected Workbook workbook { get; set; }

		/// <summary>
		/// 退出颜色
		/// </summary>
		protected readonly string exitColor;

		/// <summary>
		/// 需求文件处理类
		/// </summary>
		public DBRequirementFile()
		{
			this.config = new UserConfig(false);
			this.exitColor = config.GetConfig(ConfigKey.ExitColor);
		}
		/// <summary>
		/// 需求文件处理类
		/// </summary>
		/// <param name="filePath">文件路径</param>
		public DBRequirementFile(string filePath):this()
		{
			this.InitFile(filePath);
		}

		/// <summary>
		/// 初始化需求文件
		/// </summary>
		/// <param name="filePath">文件路径</param>
		public virtual void InitFile(string filePath)
		{
			FileStream fstream = new FileStream(filePath, FileMode.Open);			
			this.workbook = new Workbook(fstream);
		}

		/// <summary>
		/// 获取控件信息（两个文件是分开的）
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, List<ZrControlExternalInfoFromFile>> GetControlDBInfoForTrust()
		{
			var result = new Dictionary<string, List<ZrControlExternalInfoFromFile>>();
			for (int sheetIndex = 1; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
			{
				Worksheet sheet = this.workbook.Worksheets[sheetIndex];
				var list = new List<ZrControlExternalInfoFromFile>();
				for (int rowIndex = 2; rowIndex < 1000; rowIndex++)
				{
					//遇到红色背景就退出
					if (sheet.Cells[$"A{rowIndex}"].GetStyle().ForegroundColor.Name == this.exitColor)
						break;
					if (string.IsNullOrEmpty(sheet.Cells[$"A{rowIndex}"].Value.ObjToString())) continue;
					try
					{
						list.Add(
							new ZrControlExternalInfoFromFile()
							{
								ZrTable = sheet.Cells[$"A{rowIndex}"].Value.ToString(),
								ZrDescription = sheet.Cells[$"B{rowIndex}"].Value.ToString(),
								ZrField = sheet.Cells[$"C{rowIndex}"].Value.ToString(),
								TypeLength = sheet.Cells[$"D{rowIndex}"].Value.ToString(),
								Default = sheet.Cells[$"E{rowIndex}"].Value.ObjToString(),
								ZrFormat = sheet.Cells[$"F{rowIndex}"].Value.ObjToString(),
								ZrIsEnum = sheet.Cells[$"G{rowIndex}"].Value.ToString().ToBool(),
								ZrIsNotNull = sheet.Cells[$"H{rowIndex}"].Value.ToString().ToBool(),
								ZrIsReadOnly = sheet.Cells[$"I{rowIndex}"].Value.ToString().ToBool(),
								ZrVerify = sheet.Cells[$"J{rowIndex}"].Value.ObjToString()
							}
						);
					}
					catch (Exception ex)
					{
						ccCommonFunctions.Function.MsgError($"发生异常，行号：{rowIndex},异常信息:{ex.Message}");
						throw ex;
					}
					
				}
				result.Add(sheet.Name, list);
			}
			return result;
		}
	}
}
