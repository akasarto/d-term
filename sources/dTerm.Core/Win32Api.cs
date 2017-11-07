using System;
using System.Runtime.InteropServices;

namespace dTerm.Core
{
	public class Win32Api
	{
		//
		// Indexes / Styles
		//

		public const int GWL_STYLE = -16;
		public const int GWL_EXSTYLE = -20;
		public const int GWLP_HWNDPARENT = -8;

		public const long WS_POPUP = 0x80000000L;
		public const long WS_CAPTION = 0x00C00000L;
		public const long WS_DLGFRAME = 0x00400000L;
		public const long WS_MAXIMIZEBOX = 0x00010000L;
		public const long WS_MINIMIZEBOX = 0x00020000L;

		public const long WS_THICKFRAME = 0x00040000L;
		public const long WS_CHILD = 0x40000000;
		public const long WS_VISIBLE = 0x10000000;
		public const long WS_VSCROLL = 0x00200000;
		public const long WS_BORDER = 0x00800000;
		public const long WS_TABSTOP = 0x00010000L;
		public const long WS_OVERLAPPED = 0x00000000L;

		public const long WS_EX_NOACTIVATE = 0x08000000;
		public const long WS_EX_TOOLWINDOW = 0x00000080;
		public const long WS_EX_APPWINDOW = 0x40000;

		//
		// Local Methods
		//

		public static void MakeToolWindow(IntPtr targetWindoHandle)
		{
			var newStyle = GetWindowLong(targetWindoHandle, GWL_STYLE);

			newStyle &= ~WS_MAXIMIZEBOX;
			newStyle &= ~WS_MINIMIZEBOX;

			SetWindowLong(targetWindoHandle, GWL_STYLE, new IntPtr(newStyle));
		}

		public static void MakeChildWindow(IntPtr targetWindoHandle)
		{
			var newStyle = GetWindowLong(targetWindoHandle, GWL_STYLE);

			newStyle &= ~WS_MAXIMIZEBOX;
			newStyle &= ~WS_MINIMIZEBOX;
			newStyle &= ~WS_THICKFRAME;
			newStyle &= ~WS_DLGFRAME;
			newStyle &= ~WS_BORDER;
			newStyle &= ~WS_POPUP;

			newStyle |= WS_CHILD;
			newStyle |= WS_TABSTOP;

			SetWindowLong(targetWindoHandle, GWL_STYLE, new IntPtr(newStyle));
		}

		public static void RemoveFromTaskbar(IntPtr targetWindoHandle)
		{
			var newStyle = GetWindowLong(targetWindoHandle, GWL_EXSTYLE);

			newStyle &= ~WS_EX_APPWINDOW;

			SetWindowLong(targetWindoHandle, GWL_EXSTYLE, new IntPtr(newStyle));
		}

		public static void SetOwner(IntPtr targetWindoHandle, IntPtr newOwnerHandle)
		{
			SetWindowLong(targetWindoHandle, GWLP_HWNDPARENT, newOwnerHandle);
		}

		public static void TakeOwnership(IntPtr targetWindoHandle, IntPtr newOwnerHandle)
		{
			SetOwner(targetWindoHandle, newOwnerHandle);
			RemoveFromTaskbar(targetWindoHandle);
			MakeToolWindow(targetWindoHandle);
		}

		//
		// Native Methods
		//

		#region CreateWindowEx

		[DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode)]
		public static extern IntPtr CreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName, int style, int x, int y, int width, int height, IntPtr hwndParent, IntPtr hMenu, IntPtr hInst, [MarshalAs(UnmanagedType.AsAny)] object pvParam);

		#endregion

		#region DestroyWindow

		[DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Auto)]
		public static extern bool DestroyWindow(IntPtr handle);

		#endregion

		#region GetWindowLong

		public static long GetWindowLong(IntPtr hWnd, int nIndex)
		{
			if (IntPtr.Size == 4)
			{
				return GetWindowLong32(hWnd, nIndex);
			}

			return GetWindowLongPtr64(hWnd, nIndex);
		}

		[DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
		private static extern long GetWindowLong32(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
		private static extern long GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		#endregion

		#region SetActiveWindow

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		#endregion

		#region SetFocus

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetFocus(IntPtr hWnd);

		#endregion

		#region SetForegroundWindow

		[DllImport("user32.dll")]
		public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

		#endregion

		#region SetParent

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr SetParent(IntPtr childHandle, IntPtr parentHandle);

		#endregion

		#region SetWindowLong

		public static long SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return SetWindowLong32(hWnd, nIndex, dwNewLong);
			}

			return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
		}

		[DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
		private static extern long SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
		private static extern long SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		#endregion
	}
}
