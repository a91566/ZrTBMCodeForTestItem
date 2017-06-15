/*
 * 2017年6月6日 15:07:11 郑少宝 控件与字段信息
 * 
 * 土豆和马铃薯
 * 番茄和西红柿
 * 我喜欢的人和你
 */

using System;
using System.Collections.Generic;

namespace ZrTBMCodeForTestItem.ccCells
{

	/// <summary>
	/// 控件信息
	/// </summary>
	[Serializable]
	public class ZrControlExternalInfoFromFile :ZrControl.ZrControlExternalInfo, IEqualityComparer<ZrControlExternalInfoFromFile>
	{
		/// <summary>
		/// 行号
		/// </summary>
		public int RowIndex { get; set; }
		/// <summary>
		/// 所属项目（选项卡名称）
		/// </summary>
		public string SheetName { get; set; }
		/// <summary>
		/// 字段类型长度
		/// </summary>
		public string TypeLength { get; set; }
		/// <summary>
		/// 默认值
		/// </summary>
		public string Default { get; set; }
		/// <summary>
		/// 是否是下拉框（单纯的下拉框，ZrIsEnum = false）
		/// </summary>
		public bool IsComboBox { get; set; }

		public bool Equals(ZrControlExternalInfoFromFile x, ZrControlExternalInfoFromFile y)
		{			
			if (Object.ReferenceEquals(x, y)) return true;			
			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return false;
			if (x.ZrField == "PressDeputy28d")
			{
				int s = 1;
			}
			//2017年6月14日 16:49:20 郑少宝 只要字段重复就是重复
			//return x.ZrTable == y.ZrTable && x.ZrField == y.ZrField;
			return x.ZrField == y.ZrField;
		}
		

		public int GetHashCode(ZrControlExternalInfoFromFile x)
		{
			if (Object.ReferenceEquals(x, null)) return 0;			
			//int table = x.ZrTable == null ? 0 : x.ZrTable.GetHashCode();

			//2017年6月14日 16:49:20 郑少宝 只要字段重复就是重复
			int field = x.ZrField.GetHashCode();			
			return field;
		}


		
	}
}
