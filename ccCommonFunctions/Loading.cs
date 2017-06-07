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
		private bool isShowing;

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

		//public Loading()
		//{
		//	try
		//	{
		//		new System.Threading.Thread(delegate ()
		//			{
		//				isShowing = true;
		//				pLoading = Process.Start($@"{Application.StartupPath}\ccLoading.exe");
		//			}
		//		).Start();
		//	}
		//	catch (ArgumentException ex)
		//	{
		//		MessageBox.Show(ex.Message);
		//	}
		//}

		//public void HideLoading()
		//{
		//	do
		//	{
		//		if (pLoading != null)
		//		{
		//			pLoading.Kill();
		//			isShowing = false;
		//		}
		//	} while (isShowing);
		//}

	}
}
