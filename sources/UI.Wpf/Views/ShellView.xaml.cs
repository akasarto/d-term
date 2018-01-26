using App.Win32Api;
using System;
using System.Windows;
using System.Windows.Interop;
using UI.Wpf.ViewModels;

namespace UI.Wpf.Views
{
	public partial class ShellView : Window
	{
		public ShellView(ShellViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
			SourceInitialized += ShellView_SourceInitialized;
		}

		private void ShellView_SourceInitialized(object sender, System.EventArgs e)
		{
			var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
			hwndSource.AddHook(new HwndSourceHook(WndProc));
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;

			switch (message)
			{
				case WM.APPViewHighlight:
					{
						//ShowWindow(hwnd, wParam);
					}
					break;

				case WM.MOUSEACTIVATE:
					{
						handled = true;
						return new IntPtr((int)MouseActivationResult.MA_NOACTIVATE);
					}

				case WM.SETCURSOR:
					{
						var wlParam = new Win32Param()
						{
							BaseValue = (uint)lParam
						};

						var wMouseMsg = (WM)wlParam.HIWord;

						switch (wMouseMsg)
						{
							case WM.LBUTTONDOWN:
							case WM.RBUTTONDOWN:
							case WM.MBUTTONDOWN:
								{
									ShowWindow(hwnd, wParam);
									handled = true;
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
							case SysCommand.SC_CLOSE:
								{
									// IConsoleService is responsible for closing the window
									//_consoleInstance.Terminate();
									//HideWindow(hwnd);
									handled = true;
								}
								break;

							case SysCommand.SC_MINIMIZE:
								{
									//EventBus.Publish(new HideConsoleMessage(_consoleInstance));
									//HideWindow(hwnd);
									handled = true;
								}
								break;
						}
					}
					break;
			}

			return IntPtr.Zero;
		}

		private void ShowWindow(IntPtr ownerWindowHandle, IntPtr processWindowHandle)
		{
			NativeMethods.SetForegroundWindow(processWindowHandle);
			NativeMethods.SetActiveWindow(ownerWindowHandle);
			NativeMethods.SendMessage(ownerWindowHandle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			NativeMethods.SendMessage(ownerWindowHandle, (uint)WM.NCPAINT, new IntPtr(1), IntPtr.Zero);

			//EventBus.Publish(new ShowConsoleMessage(_consoleInstance));
		}
	}
}
