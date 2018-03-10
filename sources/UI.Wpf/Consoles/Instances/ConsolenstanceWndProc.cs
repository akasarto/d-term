using Shared.Kernel;
using System;
using System.Windows.Interop;
using WinApi.User32;

namespace UI.Wpf.Consoles
{
	public class ConsolenstanceWndProc
	{
		private readonly IConsoleInstanceViewModel _processInstanceViewModel;
		private readonly IntPtr _instanceViewHandle;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolenstanceWndProc(IConsoleInstanceViewModel processInstanceViewModel, HwndSource hwndSource)
		{
			_processInstanceViewModel = processInstanceViewModel;
			_instanceViewHandle = hwndSource.Handle;
		
			hwndSource.AddHook(WndProcCallback);
		}

		private void ActivateWindow(IntPtr wndHandle)
		{
			User32Methods.SetForegroundWindow(wndHandle);
			User32Methods.SetActiveWindow(_instanceViewHandle);
			Win32Api.SetVisualAsActive(_instanceViewHandle);
		}

		private IntPtr WndProcCallback(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;

			switch (message)
			{
				// https://msdn.microsoft.com/en-us/library/windows/desktop/ms645612%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
				case WM.MOUSEACTIVATE:
					{
						handled = true;
						return new IntPtr((int)MouseActivationResult.MA_NOACTIVATE);
					}

				// https://msdn.microsoft.com/en-us/library/windows/desktop/ms648382%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
				case WM.SETCURSOR:
					{
						var wlParam = new Win32Param()
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
									ActivateWindow(wParam);
								}
								break;
						}
					}
					break;

				// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646360(v=vs.85).aspx
				case WM.SYSCOMMAND:
					{
						var uCmdType = (SysCommand)wParam;

						switch (uCmdType)
						{
							case SysCommand.SC_CLOSE:
								{
									_processInstanceViewModel.TerminateProcess();
									handled = true;
								}
								break;

							case SysCommand.SC_MINIMIZE:
								{
									handled = true;
								}
								break;

							case SysCommand.SC_MAXIMIZE:
								{
									handled = true;
								}
								break;

							case SysCommand.SC_RESTORE:
								{
									//handled = true;
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
