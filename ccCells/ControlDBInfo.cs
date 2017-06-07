/*
 * 2017年6月6日 15:07:11 郑少宝 控件与字段信息
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZrTBMCodeForTestItem.ccCells
{

	public class ControlInfo222
	{
		/// <summary>
		/// 表名
		/// </summary>
		public string TableName { get; set; }
		/// <summary>
		/// 控件的页信息文本
		/// </summary>
		public List<string> ListTabPage { get; set; }
		/// <summary>
		/// 起始位置
		/// </summary>
		public Point SatrtLocation { get; set; }
		/// <summary>
		/// 限宽
		/// </summary>
		public int MaxWidth { get; set; }
		/// <summary>
		/// 行高
		/// </summary>
		public int RowHeight { get; set; }
		
	}

	/// <summary>
	/// 控件信息
	/// </summary>
	public class ControlDBInfo
	{		
		/// <summary>
		/// 表名
		/// </summary>
		public string TableName { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// 字段名称
		/// </summary>
		public string ColumnName { get; set; }
		/// <summary>
		/// 字段类型长度
		/// </summary>
		public string TypeLength { get; set; }
		/// <summary>
		/// 默认值
		/// </summary>
		public string Default { get; set; }
		/// <summary>
		/// 修约
		/// </summary>
		public string Format { get; set; }
		/// <summary>
		/// 是否枚举
		/// </summary>
		public bool IsEnum { get; set; }
		/// <summary>
		/// 不为空
		/// </summary>
		public bool IsNotNull { get; set; }
		/// <summary>
		/// 只读
		/// </summary>
		public bool IsReadOnly { get; set; }
		/// <summary>
		/// 正则验证
		/// </summary>
		public string Verify { get; set; }

	}
}
