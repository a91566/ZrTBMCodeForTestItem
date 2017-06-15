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
	public class DBRequirementFile : IDisposable
	{
		#region IDisposable
		/// <summary>
		/// 释放标志
		/// </summary>
		bool _disposed;
		/// <summary>
		/// 释放
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// 析构
		/// </summary>
		~DBRequirementFile()
		{
			Dispose(false);
		}
		/// <summary>
		/// 提供子类覆写
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			if (disposing)
			{
				if (this.config != null) this.config = null;
				if (this.workbook != null) this.workbook = null;
				if (this.fstream != null)
				{
					this.fstream.Dispose();
					this.fstream = null;
				}
			}
			_disposed = true;
		}
		#endregion

		/// <summary>
		/// 用户配置
		/// </summary>
		protected UserConfig config;
		/// <summary>
		/// 文件流
		/// </summary>
		protected FileStream fstream;
		/// <summary>
		/// 工作簿
		/// </summary>
		protected Workbook workbook;

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
			this.fstream = new FileStream(filePath, FileMode.Open);			
			this.workbook = new Workbook(fstream);
		}

		///// <summary>
		///// 获取控件信息（两个文件是分开的）
		///// </summary>
		///// <returns></returns>
		//public List<ZrControlExternalInfoFromFile> GetControlDBInfoForTrust()
		//{
		//	var result = new List<ZrControlExternalInfoFromFile>();
		//	for (int sheetIndex = 1; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
		//	{
		//		Worksheet sheet = this.workbook.Worksheets[sheetIndex];
		//		var temp = new List<ZrControlExternalInfoFromFile>();
		//		for (int rowIndex = 2; rowIndex < 1000; rowIndex++)
		//		{
		//			//遇到红色背景就退出
		//			if (sheet.Cells[$"A{rowIndex}"].GetStyle().ForegroundColor.Name == this.exitColor)
		//				break;
		//			if (string.IsNullOrEmpty(sheet.Cells[$"A{rowIndex}"].Value.ObjToString())) continue;
		//			try
		//			{
		//				temp.Add(
		//					new ZrControlExternalInfoFromFile()
		//					{
		//						ZrTable = sheet.Cells[$"A{rowIndex}"].Value.ToString(),
		//						ZrDescription = sheet.Cells[$"B{rowIndex}"].Value.ToString(),
		//						ZrField = sheet.Cells[$"C{rowIndex}"].Value.ToString(),
		//						TypeLength = sheet.Cells[$"D{rowIndex}"].Value.ToString(),
		//						Default = sheet.Cells[$"E{rowIndex}"].Value.ObjToString(),
		//						ZrFormat = sheet.Cells[$"F{rowIndex}"].Value.ObjToString(),
		//						ZrIsEnum = sheet.Cells[$"G{rowIndex}"].Value.ToString().Contains("枚举"),
		//						IsComboBox = sheet.Cells[$"G{rowIndex}"].Value.ObjToBool(),
		//						ZrIsNotNull = sheet.Cells[$"H{rowIndex}"].Value.ObjToBool(),
		//						ZrIsReadOnly = sheet.Cells[$"I{rowIndex}"].Value.ObjToBool(),
		//						ZrVerify = sheet.Cells[$"J{rowIndex}"].Value.ObjToString()
		//					}
		//				);
		//			}
		//			catch (Exception ex)
		//			{
		//				ccCommonFunctions.Function.MsgError($"发生异常，行号：{rowIndex}, 异常信息:{ex.Message}");
		//				throw ex;
		//			}					
		//		}
		//		//result.AddRange(temp.Clone() as List<ZrControlExternalInfoFromFile>);
		//		result.AddRange(temp);
		//	}
		//	return result;
		//}

		/// <summary>
		/// 获取控件信息（两个文件是分开的）
		/// </summary>
		/// <returns></returns>
		public void GetControlDBInfoFromFile(ref List<ZrControlExternalInfoFromFile> list)
		{
			//从第二页开始
			for (int sheetIndex = 1; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
			{
				Worksheet sheet = this.workbook.Worksheets[sheetIndex];
				for (int rowIndex = 2; rowIndex < 1000; rowIndex++)
				{
					if (rowIndex == 90)
					{
						int sss = 1;
					}
					//遇到红色背景就退出
					if (sheet.Cells[$"A{rowIndex}"].GetStyle().ForegroundColor.Name == this.exitColor)
						break;
					if (string.IsNullOrEmpty(sheet.Cells[$"A{rowIndex}"].Value.ObjToString())) continue;
					try
					{
						list.Add(
							new ZrControlExternalInfoFromFile()
							{
								RowIndex = rowIndex,
								SheetName = sheet.Name,
								ZrTable = sheet.Cells[$"A{rowIndex}"].Value.ToString().Trim(),
								ZrDescription = sheet.Cells[$"B{rowIndex}"].Value.ToString().Trim(),
								ZrField = sheet.Cells[$"C{rowIndex}"].Value.ToString().Trim(),
								TypeLength = sheet.Cells[$"D{rowIndex}"].Value.ToString().Trim(),
								Default = sheet.Cells[$"E{rowIndex}"].Value.ObjToString().Trim(),
								ZrFormat = sheet.Cells[$"F{rowIndex}"].Value.ObjToString().Trim(),
								ZrIsEnum = sheet.Cells[$"G{rowIndex}"].Value.ObjToBool(),
								ZrIsNotNull = sheet.Cells[$"H{rowIndex}"].Value.ObjToBool(),
								ZrIsReadOnly = sheet.Cells[$"I{rowIndex}"].Value.ObjToBool(),
								ZrVerify = sheet.Cells[$"J{rowIndex}"].Value.ObjToString().Trim()
							}
						);
					}
					catch (Exception ex)
					{
						ccCommonFunctions.Function.MsgError($"发生异常，行号：{rowIndex},异常信息:{ex.Message}");
						throw ex;
					}
				}
			}
		}
	}
}
