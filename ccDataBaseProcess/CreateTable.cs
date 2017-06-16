/*
 * 2017年6月13日 11:42:48 郑少宝 创建表
 * 
 * 遇见你是多小的概率我不想知道
 * 我只想把失去你的概率变为零 
 */
using System.Collections.Generic;
using System.Linq;
using ZrTBMCodeForTestItem.ccCells;

namespace ccZrCHKCodeProduction.ccDataBaseProcess
{
	public class CreateTable
	{
		/// <summary>
		/// 字段信息
		/// </summary>
		public List<ZrControlExternalInfoFromFile> ListZrControlExternalInfoFromFile { get; set; }

		/// <summary>
		/// 导出脚本
		/// </summary>
		/// <returns></returns>
		public (bool result, string errorInfo, List<string> scripts) Export()
		{
			bool result = false;
			string errorInfo = null;
			List<string> scripts = null;
			var tables = getTableName();
			if (tables.Count == 0)
			{
				errorInfo = "没有获取到表名。";
			}
			else
			{
				result = true;
				scripts = this.createScript(tables, true);
			}
			return (result: result, errorInfo: errorInfo, scripts: scripts);
		}

		/// <summary>
		/// 导出脚本
		/// </summary>
		/// <param name="fileName">文件名</param>
		/// <returns></returns>
		public (bool result, string errorInfo) Export(string fileName)
		{
			bool result = false;
			string errorInfo = null;
			List<string> scripts = null;
			var tables = getTableName();
			if (tables.Count == 0)
			{
				errorInfo = "没有获取到表名。";
			}
			else
			{
				scripts = this.createScript(tables, true);
			}
			ZrTBMCodeForTestItem.ccCommonFunctions.Function.WriteFile(scripts, fileName, true);
			result = true;
			return (result: result, errorInfo: errorInfo);
		}

		/// <summary>
		/// 执行脚本
		/// </summary>
		/// <param name="fileName">文件名</param>
		/// <returns></returns>
		public (bool result, string errorInfo) ExSQL(string linkString)
		{
			bool result = false;
			string errorInfo = null;
			List<string> scripts = null;
			var tables = getTableName();
			if (tables.Count == 0)
			{
				errorInfo = "没有获取到表名。";
			}
			else
			{
				scripts = this.createScript(tables,false);
			}
			zsbApps.DBHelper.DBHelper db = zsbApps.DBHelper.DBHelper.Instance(zsbApps.DBHelper.DBClassify.MsSql, linkString);
			string exsql = string.Join(" ", scripts);
			db.ExecuteSql(exsql);
			result = true;
			return (result: result, errorInfo: errorInfo);
		}

		/// <summary>
		/// 获取表名
		/// </summary>
		/// <returns></returns>
		private List<string> getTableName()
		{
			if (this.ListZrControlExternalInfoFromFile == null || this.ListZrControlExternalInfoFromFile.Count == 0) return null;
			List<string> result = new List<string>();
			string tableName = null;
			foreach (var item in this.ListZrControlExternalInfoFromFile)
			{
				if (string.IsNullOrEmpty(tableName))
					tableName = item.ZrTable;
			}
			result.Add(tableName);
			// 这里用 Distinct 去重不可用
			var temp = this.ListZrControlExternalInfoFromFile.Where(i => i.ZrTable != tableName);
			if (temp.Count() > 0)
			{
				foreach (var item in temp)
				{
					if (!result.Contains(item.ZrTable))
						result.Add(item.ZrTable);
				}
			}
			return result;
		}


		/// <summary>
		/// 获取表的字段信息
		/// </summary>
		/// <param name="tables">表名集合</param>
		/// <param name="addNote">是否生成注释</param>
		/// <returns></returns>
		private List<string> createScript(List<string> tables, bool addNote)
		{
			List<string> result = new List<string>();
			foreach (var item in tables)
			{
				result.AddRange(createScript(item, true, addNote));
			}
			return result;
		}

		/// <summary>
		/// 生成创建表的脚本
		/// </summary>
		/// <param name="table">表名</param>
		/// <param name="commonField">公共字段</param>
		/// <param name="addNote">是否生成注释</param>
		/// <returns></returns>
		private List<string> createScript(string table, bool commonField, bool addNote)
		{
			var result = new List<string>();
			var listDescription = new List<string>();
			if (addNote)
			{
				result.Add("");
				result.Add("");
				result.Add("-- ---------------------------------------------------------");
				result.Add($"-- Table structure for {table}");
				result.Add("-- ---------------------------------------------------------");
			}
			result.Add($"IF EXISTS ( SELECT id FROM sysobjects WHERE xtype='u' AND name='{table}')");
			result.Add($"BEGIN");
			result.Add($"	DROP TABLE [dbo].[{table}]");
			result.Add($"END");
			if (addNote) result.Add("GO");
			result.Add($"CREATE TABLE [dbo].[{table}] (");
			result.Add("	[RowID] int NOT NULL IDENTITY(1,1),");
			result.Add("	[SampleID] varchar(30) NOT NULL,");
			result.Add("	[IsDel] tinyint NOT NULL DEFAULT ((0)),");
			result.Add("	[CreateDateTime] datetime NOT NULL DEFAULT (getdate()),");			

			var data = ListZrControlExternalInfoFromFile.Where(i => i.ZrTable == table);
			foreach (ZrControlExternalInfoFromFile item in data)
			{
				result.Add(string.Format("	[{0}] {1} {2} {3},", item.ZrField, item.TypeLength,
					item.ZrIsNotNull ? "NOT NULL" : "NULL",
					string.IsNullOrEmpty(item.Default) ? "" : $"DEFAULT({item.Default})"));
				if (!string.IsNullOrEmpty(item.ZrDescription))
				{
					listDescription.Add(string.Format("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{0}',@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'{1}', @level2type=N'COLUMN', @level2name=N'{2}'",
						item.ZrDescription, table, item.ZrField));
				}
			}
			result.Add(")");
			if (addNote) result.Add("GO");
			result.Add("");
			if (addNote)
			{
				result.Add("-- ---------------------------------------------------------");
				result.Add($"-- MS_Description for {table}");
				result.Add("-- ---------------------------------------------------------");
			}
			result.AddRange(listDescription);
			if (addNote) result.Add("GO");
			result.Add("");
			if (addNote)
			{
				result.Add("-- ---------------------------------------------------------");
				result.Add($"-- Primary Key structure for {table}");
				result.Add("-- ---------------------------------------------------------");
			}
			result.Add($"ALTER TABLE [dbo].[{table}] ADD PRIMARY KEY ([RowID])");
			if (addNote) result.Add("GO");

			return result;
		}

		
	}
}
