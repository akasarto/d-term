using System;
using System.Runtime.InteropServices;
using System.Text;
using WinApi.User32;

namespace UI.Wpf
{
	internal static class Win32Api
	{
		internal static void SetVisualAsActive(IntPtr handle)
		{
			PostMessage(handle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			PostMessage(handle, (uint)WM.NCPAINT, new IntPtr(1), IntPtr.Zero);
		}

		internal static bool IsConsoleProcess(IntPtr hWnd)
		{
			int outLength;
			var stringBuilder = new StringBuilder(256);

			outLength = User32Methods.GetClassName(hWnd, stringBuilder, stringBuilder.Capacity);

			if (outLength != 0)
			{
				return stringBuilder.ToString().ToLower().Contains("consolewindowclass");
			}
			else
			{
				return false;
			}
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
	}
}
