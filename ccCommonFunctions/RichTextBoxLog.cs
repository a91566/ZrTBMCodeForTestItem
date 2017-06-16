/*
 * 2017年6月16日 21:07:24 郑少宝 日志输出
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZrTBMCodeForTestItem.ccCommonFunctions
{
	public enum LogLevel
	{
		/// <summary>
		/// 普通消息
		/// </summary>
		Info,
		/// <summary>
		/// 警告消息
		/// </summary>
		Waring,
		/// <summary>
		/// 错误消息
		/// </summary>
		Error,
		/// <summary>
		/// 致命错误
		/// </summary>
		FatalError
	}
	/// <summary>
	/// 输出日志到富文本框
	/// </summary>
	public class RichTextBoxLog
	{
		private Color[] colors = new Color[] { Color.Gray, Color.Black, Color.Red, Color.DarkRed};
		/// <summary>
		/// 要输出的控件，设为属性，便于切换
		/// </summary>
		public RichTextBox RichTextBox { get; set; }

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="rtb"></param>
		public RichTextBoxLog(RichTextBox rtb)
		{
			this.RichTextBox = rtb;
		}
		

		/// <summary> 
		/// 追加显示文本 
		/// </summary> 
		/// <param name="text">日志内容</param> 
		/// <param name="level">级别</param> 
		public void LogAppend(string text, LogLevel level = LogLevel.Info)
		{			
			this.RichTextBox.Invoke(new Action(() =>
			{
				this.RichTextBox.SelectionColor = colors[(int)level];
				this.RichTextBox.AppendText($"{DateTime.Now.ToString("HH:mm:ss")}:{Environment.NewLine}	{text}");
				this.RichTextBox.AppendText(Environment.NewLine);
			}));
		}

		/// <summary> 
		/// 追加显示文本 
		/// </summary> 
		/// <param name="list">日志内容</param> 
		/// <param name="level">级别</param> 
		public void LogAppend(List<string> list, LogLevel level = LogLevel.Info)
		{
			this.RichTextBox.Invoke(new Action(() =>
			{
				this.RichTextBox.SelectionColor = colors[(int)level];
				this.RichTextBox.AppendText($"{DateTime.Now.ToString("HH:mm:ss")}:{Environment.NewLine}");
				foreach (var item in list)
				{
					this.RichTextBox.SelectionColor = colors[(int)level];
					this.RichTextBox.AppendText($"	{item}{Environment.NewLine}");
				}
			}));
		}


	}
}
