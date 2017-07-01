/*
 * 2017年6月6日 14:16:22 郑少宝 扩展方法
 * 此扩展类 针对中润代码生成器，其他未必适用，涉及C#7.0语法，如需使用删除或修改对应的语法即可
 * 
 * 从来不偷东西的你，却爱偷笑
 * 于是从来不偷东西的我
 * 便学会了偷看
 */
using System.Text.RegularExpressions;

namespace ZrTBMCodeForTestItem.ccExtend
{
	public static class ExtendString
    {
		/// <summary>
		/// 将文本转为转为 bool 类型, (1、是、true,返回 true，其余返回 false)
		/// </summary>
		/// <param name="s">待处理的字符串</param>
		/// <returns>bool 值</returns>
		public static bool ToBool(this string s)
		{
			if (string.IsNullOrEmpty(s)) return false;
			if (s == "1" || s == "是" || s.ToLower() == "true") return true;
			return false;
		}

		/// <summary>
		/// 将文本转为 int 数据， null 返回 0
		/// </summary>
		/// <param name="s">待处理的字符串</param>
		/// <returns>整型数据</returns>
		public static int ToInt(this string s)
		{
			if (int.TryParse(s, out int x))
			{
				return x;
			}
			return 0;
		}

		/// <summary>
		/// 将文本转为 int 数据， null 返回 0
		/// </summary>
		/// <param name="s">待处理的字符串</param>
		/// <returns>整型数据</returns>
		public static (int decimalLength, int decimalPart) ToDecimalLength(this string s)
		{
			string patten = @"\([^()]+\)";
			Match match = new Regex(patten).Match(s);
			if (match.Success)
			{
				var result = match.Value.Replace("(","").Replace(")","").Split(new char[] { ',' });
				return (decimalLength: result[0].ToInt(), decimalPart: result[1].ToInt());
			}
			return (decimalLength: 0, decimalPart: 0);
		}

		/// <summary>
		/// 获取文本中的数字 主要针对 数据库设计，
		/// 如 nvarchar(20), 返回20
		/// 如 varchar(40), 返回40
		/// 如 datetime, 返回0
		/// </summary>
		/// <param name="s">待处理的字符串</param>
		/// <returns>整型数据</returns>
		public static int GetInt(this string s)
		{
			string patten = @"[1-9]\d";
			Match match = new Regex(patten).Match(s);
			if (match.Success)
			{
				return ToInt(match.Value);
			}			
			return 0;
		}

		/// <summary>
		/// 将 object 转为 string，null 返回 ""
		/// </summary>
		/// <param name="obj">待处理的对象</param>
		/// <returns>字符串</returns>
		public static string ObjToString(this object obj)
		{
			return obj == null ? "" : obj.ToString();
		}

		/// <summary>
		/// 将 object 转为 bool，(1、是、true,返回 true，其余返回 false)
		/// </summary>
		/// <param name="obj">待处理的对象</param>
		/// <returns></returns>
		public static bool ObjToBool(this object obj)
		{
			return obj == null ? false : obj.ToString().Trim().ToBool();
		}

		/// <summary>
		/// 去掉换行符
		/// </summary>
		/// <param name="text">待处理的对象</param>
		/// <returns></returns>
		public static string RemoveNewLine(this string text)
		{
			return text.Replace(System.Environment.NewLine, "");
		}

		/// <summary>
		/// 去掉空格
		/// </summary>
		/// <param name="text">待处理的对象</param>
		/// <returns></returns>
		public static string RemoveEmptyChar(this string text)
		{
			return text.Replace(" ", "").Replace(" ", "");
		}

		/// <summary>
		/// 移除特殊字符
		/// </summary>
		/// <param name="text">待处理的对象</param>
		/// <returns>字符串</returns>
		public static string RemoveSpecialChar(this string text)
		{
			//"lbl面积(mm²)" return lbl面积mm
			//"lbl达到设计强度(%)" return lbl达到设计强度
			//2017年6月19日 16:15:54 用正则去匹配，然后删除括号

			//先替换特殊字符，再正则移除特殊字符
			text = text.Replace("(", "").Replace(")", "")
				.Replace("（", "").Replace("）", "")
				.Replace("+", "").Replace("-", "")
				.Replace("÷", "").Replace("×", "")
				.Replace("²", "").Replace("³", "")
				.Replace("-", "")
				.Replace("%", "")
				.Replace("：", "")
				.Replace(":", "")
				.Replace("*", "")
				.Replace("#", "")
				.Replace("@", "")
				.Replace("<", "")
				.Replace(">", "")
				.Replace("/", "")
				.Replace("℃", "");

			string patten = @"^[a-zA-Z]+[\u4e00-\u9fa5]+[a-zA-Z0-9]+";
			Match match = new Regex(patten).Match(text); 
			if (match.Success)
			{
				return match.Value.Trim();
			}
			return text.Trim();			
		}

		/// <summary>
		/// 移除特殊字符
		/// </summary>
		/// <param name="text">待处理的对象</param>
		/// <param name="chars">自定义的特殊字符串数组</param>
		/// <returns>字符串</returns>
		public static string RemoveSpecialChar(this string text, string[] chars)
		{
			if (chars != null)
			{
				foreach (var item in chars)
				{
					text = text.Replace(item, "");
				}
				string patten = @"^[a-zA-Z]+[\u4e00-\u9fa5]+[a-zA-Z0-9]+";
				Match match = new Regex(patten).Match(text);
				if (match.Success)
				{
					return match.Value;
				}
				return text;
			}
			else
			{
				return text.RemoveSpecialChar();
			}			
		}


	}
}
