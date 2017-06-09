/*
 * 2017年6月5日 22:18:24 郑少宝
 */
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ZrTBMCodeForTestItem.ccCommonFunctions
{
	public class Loading
	{
		private Process pLoading;

		public Loading(IntPtr hwd)
		{
			try
			{
				StartProcess($@"{Application.StartupPath}\ccLoading.exe", new string[1] { hwd.ToString() }); 
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void HideLoading()
		{
			if (pLoading != null)
			{
				try
				{
					pLoading.Kill();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}



		public bool StartProcess(string filename, string[] args)
		{
			try
			{
				pLoading = new Process();
				ProcessStartInfo startInfo = new ProcessStartInfo(filename, string.Join(" ", args));
				pLoading.StartInfo = startInfo;
				//通过以下参数可以控制exe的启动方式，具体参照 myprocess.StartInfo.下面的参数，如以无界面方式启动exe等
				pLoading.StartInfo.UseShellExecute = false;
				pLoading.Start();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("loading error：" + ex.Message);
			}
			return false;
		}



	}
}
