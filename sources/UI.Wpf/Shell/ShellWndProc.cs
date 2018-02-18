using ReactiveUI;
using Shared.Kernel;
using System;
using System.Windows.Interop;
using UI.Wpf.Consoles;
using WinApi.User32;

namespace UI.Wpf.Shell
{
	public class ShellWndProc
	{
		private IntPtr _shellWindowHandle;
		private IntPtr _latestActiveConsoleHandle;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellWndProc(HwndSource shellHwndSource)
		{
			_shellWindowHandle = shellHwndSource.Handle;
			shellHwndSource.AddHook(WndProc);

			MessageBus.Current.Listen<ConsoleProcessCreatedMessage>().Subscribe(message =>
			{
				_latestActiveConsoleHandle = message.NewConsoleProcess.MainWindowHandle;
			});
		}

		/// <summary>
		/// Activate the given handle active forcing the shell view to look active as well.
		/// </summary>
		/// <param name="wndHandle">Either the shell or a console child handle.</param>
		private void ActivateWindow(IntPtr wndHandle)
		{
			User32Methods.SetActiveWindow(_shellWindowHandle);
			User32Methods.SetForegroundWindow(_shellWindowHandle);
			User32Methods.SetForegroundWindow(wndHandle);

			SetShellVisualAsActive();
		}

		/// <summary>
		/// Activate the latest console.
		/// </summary>
		private void RestoreLastActivatedWindow() => ActivateWindow(_latestActiveConsoleHandle);

		/// <summary>
		/// Force the shell view to be displayed as active.
		/// </summary>
		private void SetShellVisualAsActive() => Win32Api.SetVisualAsActive(_shellWindowHandle);

		/// <summary>
		/// Handle windows native api messages.
		/// </summary>
		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;

			switch (message)
			{
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
									if (wParam != _shellWindowHandle)
									{
										_latestActiveConsoleHandle = wParam;
										ActivateWindow(wParam);
									}
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
							case SysCommand.SC_MAXIMIZE:
							case SysCommand.SC_RESTORE:
								{
									RestoreLastActivatedWindow();
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
