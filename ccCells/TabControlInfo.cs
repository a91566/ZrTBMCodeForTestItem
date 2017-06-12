/*
 * 2017年6月11日 12:58:54 郑少宝 内嵌页的信息
 * 
 * 我那么爱吃醋不是因为不相信你
 * 而是你在我心中太美好
 * 尽管你并没那么优秀
 */
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZrTBMCodeForTestItem.ccCells
{
	public class TabControlInfo
	{
		public TabControl TabControl;
		/// <summary>
		/// 用一个标识来存储是否处理内嵌页
		/// </summary>
		public bool IsOperateTabControl;
		/// <summary>
		/// 内嵌页颜色区分（用颜色区别页）
		/// </summary>
		public List<string> ListBackgroundColorName { get; set; }
		/// <summary>
		/// 内嵌页的起始 列 X、行号 Y， 注意不是从 0 开始，而是行号
		/// </summary>
		public List<Point> ColumnRowIndex { get; set; }
		/// <summary>
		/// 内嵌页的结束 列 X、行号 Y， 注意不是从 0 开始，而是行号，就是上面一个加下面一个
		/// </summary>
		public List<Point> ColumnRowIndexEnd { get; set; }
		/// <summary>
		/// 内嵌页所占的 列数 width 与 行数 height，注意这个从 0 开始
		/// </summary>
		public List<Size> ColumnRowCount { get; set; }
	}
}
