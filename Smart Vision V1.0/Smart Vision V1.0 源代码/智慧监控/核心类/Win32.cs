using System;
using System.Runtime.InteropServices;

namespace IPCamera
{
	/// <summary>
	/// Win32API����
	/// </summary>
	public class Win32
	{
		// GetSystemMetrics - ��ȡϵͳ��Ļ�ߴ��������Ϣ
		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics( [MarshalAs(UnmanagedType.I4)] SystemMetrics metric);

		// ϵͳ��Ļ��Ϣ
		public enum SystemMetrics
		{
			CXSCREEN	= 0,
			CYSCREEN	= 1,
			CYCAPTION	= 4,
			CYMENU		= 15,
			CXFRAME		= 32,
			CYFRAME		= 33
		}
	}
}
