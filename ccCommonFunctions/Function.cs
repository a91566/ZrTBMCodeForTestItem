/*
 * 2017年6月2日 09:59:02 郑少宝 导出文件
 * 
 * 我怀旧，因为我看不到你和未来。
 * 
 */using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ZrTBMCodeForTestItem.ccCommonFunctions
{
    public static class Function
    {
		#region 文件读写
		/// <summary>
		/// 写文件
		/// </summary>
		/// <param name="list">列表内容</param>
		/// <param name="filepath">文件全路径</param>
		public static void WriteFile(List<string> list, string filepath)
		{
			using (FileStream fs = new FileStream(filepath, FileMode.Create))
			{
				using (StreamWriter sw = new StreamWriter(fs))
				{
					foreach (string item in list)
					{
						sw.WriteLine(item);
					}
					sw.Flush();
				}
			}
		}

		/// <summary>
		/// 写文件
		/// </summary>
		/// <param name="words">内容数组</param>
		/// <param name="filepath">文件全路径</param>
		public static void WriteFile(string[] words, string filepath)
		{
			using (FileStream fs = new FileStream(filepath, FileMode.Create))
			{
				using (StreamWriter sw = new StreamWriter(fs))
				{
					foreach (string item in words)
					{
						sw.WriteLine(item);
					}
					sw.Flush();
				}
			}
		}

		/// <summary>
		/// 写文件
		/// </summary>
		/// <param name="words">内容数组</param>
		/// <param name="filepath">文件全路径</param>
		/// <param name="header">写入头部标识文件</param>
		public static void WriteFile(string[] words, string filepath, bool header)
		{
			if (header)
			{				
				List<string> list = new List<string>()
				{
					"/*",
					$" * {DateTime.Now.ToString("yyyy年M月d日 HH:mm:ss")} 中润代码生成器",
					" *",
					" */"
				};
				list.AddRange(words);
				WriteFile(list, filepath);
			}
			else
			{
				WriteFile(words, filepath);
			}
		}

		/// <summary>
		/// 读取文件到到 List
		/// </summary>
		/// <param name="filepath"></param>
		/// <returns></returns>
		public static string[] ReadFile(string filepath)
		{
			return File.ReadAllLines(filepath, Encoding.UTF8);
		}
		#endregion

		#region 对话框
		/// <summary>
		/// 信息提示
		/// </summary>
		/// <param name="strShow">提示内容</param>
		/// <param name="strTitle">标题</param>
		public static void MsgInfo(string strShow, string strTitle = "系统消息")
		{
			MessageBox.Show(strShow, strTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// 错误提示
		/// </summary>
		/// <param name="strShow">提示内容</param>
		/// <param name="strTitle">标题</param>
		public static void MsgError(string strShow, string strTitle = "系统消息")
		{
			MessageBox.Show(strShow, strTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// 警告提示
		/// </summary>
		/// <param name="strShow">提示内容</param>
		/// <param name="strTitle">标题</param>
		public static void MsgWarm(string strShow, string strTitle = "系统消息")
		{
			MessageBox.Show(strShow, strTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
		/// <summary>
		/// 询问框
		/// </summary>
		/// <param name="strShow">询问内容</param>
		/// <param name="strTitle">标题</param>
		/// <returns>true/false</returns>
		public static bool MsgOK(string strShow, string strTitle = "系统消息")
		{
			DialogResult dr;
			dr = MessageBox.Show(strShow, strTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (dr == DialogResult.OK)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion
	}
}
