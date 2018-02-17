using Sarto.Extensions;
using WinApi;
using System;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace UI.Wpf.Shell
{
	public class ShellWndProc
	{
		private IntPtr _shellViewHandle;
		private IntPtr _lastForegroundHandle;

		public ShellWndProc(HwndSource shellHwndSource)
		{
			_shellViewHandle = shellHwndSource.Handle;
			shellHwndSource.AddHook(WndProc);
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		private void SetShellActive()
		{
			PostMessage(_shellViewHandle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			PostMessage(_shellViewHandle, (uint)WM.NCPAINT, new IntPtr(1), IntPtr.Zero);
		}

		private void SetForegroundHandle(IntPtr wndHandle)
		{
			if (wndHandle != _shellViewHandle)
			{
				_lastForegroundHandle = wndHandle;
			}
			User32Interop.SetActiveWindow(_shellViewHandle);
			User32Interop.SetForegroundWindow(_shellViewHandle);
			User32Interop.SetForegroundWindow(wndHandle);
			SetShellActive();
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;

			switch (message)
			{
				case WM.ACTIVATEAPP:
					{
						if (hwnd == _shellViewHandle)
						{
							var activated = wParam.ToInt32().ChangeType<bool>();

							if (!activated)
							{
								handled = true;
								SetShellActive();
								return IntPtr.Zero;
							}
						}
					}
					break;

				case WM.MOUSEACTIVATE:
					{
						handled = true;
						SetShellActive();
						return new IntPtr((int)MouseActivationResult.MA_NOACTIVATE);
					}

				case WM.SETCURSOR:
					{
						var wlParam = new MsgParam()
						{
							BaseValue = (uint)lParam
						};

						var wMouseMsg = (WM)wlParam.HOWord;

						switch (wMouseMsg)
						{
							case WM.LBUTTONDOWN:
							case WM.RBUTTONDOWN:
							case WM.MBUTTONDOWN:
								{
									SetForegroundHandle(wParam);
								}
								break;
						}
					}
					break;

				case WM.SYSCOMMAND:
					{
						var uCmdType = (SysCommand)wParam;

						switch (uCmdType)
						{
							case SysCommand.SC_MAXIMIZE:
							case SysCommand.SC_RESTORE:
								{
									SetForegroundHandle(_lastForegroundHandle);
								}
								break;
						}
					}
					break;
			}

			return IntPtr.Zero;
		}
	}
}
