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

		public Loading()
		{
			try
			{
				pLoading = Process.Start($@"{Application.StartupPath}\ccLoading.exe");
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void HideLoading()
		{
			if (pLoading != null)
				pLoading.Kill();
		}

	}
}
