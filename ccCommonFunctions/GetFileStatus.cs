﻿/*
 * 2017年6月12日 17:13:37 郑少宝 查看文件状态
 * 
 * 只要你肯转头
 * 我一直为你静候
 * 一生为期
 */
using System;
using System.Runtime.InteropServices;

namespace ZsbApps
{
	public class GetFileStatus
	{
		[DllImport("kernel32.dll")]
		public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr hObject);

		public const int OF_READWRITE = 2;
		public const int OF_SHARE_DENY_NONE = 0x40;
		public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

		/// <summary>
		/// 查看文件是否被占用
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool IsFileOccupied(string filePath)
		{
			IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
			CloseHandle(vHandle);
			return vHandle == HFILE_ERROR ? true : false;
		}
	}
}
