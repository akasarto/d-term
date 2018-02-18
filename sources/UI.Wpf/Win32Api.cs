using Shared.Kernel;
using System;
using System.Runtime.InteropServices;
using WinApi.User32;

namespace UI.Wpf
{
	internal delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

	internal static class Win32Api
	{
		/// <summary>
		/// Send win messages to paint the corresponding window as active.
		/// </summary>
		/// <param name="handle">The pointer to the window that should be painted as active.</param>
		internal static void SetVisualAsActive(IntPtr handle)
		{
			PostMessage(handle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			PostMessage(handle, (uint)WM.NCPAINT, new IntPtr(1), IntPtr.Zero);
		}

		internal static IntPtr FindWindowHandle(uint windowThreadId)
		{
			uint threadId = 0;
			IntPtr windowHandle = IntPtr.Zero;

			do
			{
				windowHandle = User32Methods.FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = User32Methods.GetWindowThreadProcessId(windowHandle, IntPtr.Zero);
				if (threadId == windowThreadId)
				{
					return windowHandle;
				}
			} while (!windowHandle.Equals(IntPtr.Zero));

			return IntPtr.Zero;
		}

		[DllImport("user32.dll", CharSet = Win32Charset.Current)]
		internal static extern bool EnumThreadWindows(IntPtr dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true, CharSet = Win32Charset.Current)]
		internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
	}
}
