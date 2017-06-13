/*
 * 2017年6月13日 22:40:57 郑少宝
 */
using System;
using System.Windows.Forms;

namespace ZsbApps
{
	public class UnlockerFile
	{
		public static bool Unlock(string filePath)
		{
			try
			{
				System.Diagnostics.Process open = new System.Diagnostics.Process();
				open.StartInfo.FileName = $@"{Application.StartupPath}\Unlocker.exe";
				open.StartInfo.Arguments = $@"{filePath} /s";
				open.Start();
			}
			catch (Exception ex)
			{
				return false;
				throw ex;
			}
			return true;
		}
	}
}
