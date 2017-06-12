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
	public class ZrControlExternalInfoFromFile :ZrControl.ZrControlExternalInfo, IEqualityComparer<ZrControlExternalInfoFromFile>
	{	
		/// <summary>
		/// 字段类型长度
		/// </summary>
		public string TypeLength { get; set; }
		/// <summary>
		/// 默认值
		/// </summary>
		public string Default { get; set; }

		public bool Equals(ZrControlExternalInfoFromFile x, ZrControlExternalInfoFromFile y)
		{			
			if (Object.ReferenceEquals(x, y)) return true;			
			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return false;
			
			return x.ZrTable == y.ZrTable && x.ZrField == y.ZrField;
		}
		

		public int GetHashCode(ZrControlExternalInfoFromFile x)
		{
			if (Object.ReferenceEquals(x, null)) return 0;			
			int table = x.ZrTable == null ? 0 : x.ZrTable.GetHashCode();			
			int field = x.ZrField.GetHashCode();			
			return table ^ field;
		}
	}
}
