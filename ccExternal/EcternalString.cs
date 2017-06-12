﻿/*
 * 2017年6月6日 14:16:22 郑少宝 扩展方法
 * 此扩展类 针对中润代码生成器，其他未必适用
 * 
 * 从来不偷东西的你，却爱偷笑
 * 于是从来不偷东西的我
 * 便学会了偷看
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZrTBMCodeForTestItem.ccEcternal
{
    public static class EcternalString
    {
		/// <summary>
		/// 将文本转为转为 bool 类型
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
			string patten = @"^[1-9]\d*$";
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
	}
}
