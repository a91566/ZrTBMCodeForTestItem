/*
 * 2017年6月15日 14:45:01 郑少宝 对文本框的扩展
 * 
 * 我在外求学
 * 他开始浏览一个陌生学校的网页
 * 他开始关注一个陌生城市的天气
 */
using System.Windows.Forms;

namespace ZrTBMCodeForTestItem.ccEcternal
{
	public static class ExternalTextBox
	{
		/// <summary>
		/// 设置文本框只允许输入整型数据
		/// </summary>
		/// <param name="txb">文本框</param>
		public static void SetTextBoxInt(this TextBox txb)
		{
			txb.KeyPress += (s, e) =>
			{
				if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
					e.Handled = true;
			};
		}

		/// <summary>
		/// 设置文本框只允许输入浮点型数据
		/// </summary>
		/// <param name="txb">文本框</param>
		public static void SetTextBoxFloat(this TextBox txb)
		{
			txb.KeyPress += (s, e) =>
			{
				if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 46) && (e.KeyChar != 8))
					e.Handled = true;
			};
		}
	}
}
