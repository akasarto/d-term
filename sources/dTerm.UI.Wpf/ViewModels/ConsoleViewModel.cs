using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using System;
using System.Windows.Interop;
using WinApi.User32;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ConsoleViewModel : ObservableObject, IDisposable
	{
		private string _title;
		private IntPtr _consoleViewHandle;
		private IConsoleInstance _consoleInstance;
		private ConsoleHwndHost _consoleHwndHost;

		public ConsoleViewModel(IConsoleInstance consoleInstance)
		{
			_consoleInstance = consoleInstance ?? throw new ArgumentNullException(nameof(consoleInstance), nameof(ConsoleViewModel));

			SetTitle();
		}

		public string Title
		{
			get => _title;
			set => Set(ref _title, value);
		}

		public ConsoleType Type { get; }

		public IntPtr ConsoleViewHandle
		{
			get => _consoleViewHandle;
			set
			{
				if (_consoleViewHandle != value)
				{
					Set(ref _consoleViewHandle, value);

					DisableMaximizeButton();
					SetWindowMessagesHook();
				}
			}
		}

		public IConsoleInstance Instance => _consoleInstance;

		public ConsoleHwndHost ConsoleHwndHost
		{
			get
			{
				if (_consoleHwndHost == null)
				{
					_consoleHwndHost = new ConsoleHwndHost(_consoleInstance);
				}

				return _consoleHwndHost;
			}
		}

		private void SetTitle()
		{
			Title = $"[PID {_consoleInstance.ProcessId}] {_consoleInstance.Name}";
		}

		private void DisableMaximizeButton()
		{
			var newStyle = (WindowStyles)User32Helpers.GetWindowLongPtr(_consoleViewHandle, WindowLongFlags.GWL_STYLE);

			newStyle &= ~WindowStyles.WS_MAXIMIZEBOX;

			User32Helpers.SetWindowLongPtr(_consoleViewHandle, WindowLongFlags.GWL_STYLE, new IntPtr((long)newStyle));
		}

		private void SetWindowMessagesHook()
		{
			var hwndSource = HwndSource.FromHwnd(_consoleViewHandle);
			hwndSource.AddHook(new HwndSourceHook(WndProc));
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;

			switch (message)
			{
				case (WM.APP + 0x1): // View Highlight (Custom)
					{
						Show(hwnd, wParam);
						break;
					}

				case WM.SYSCOMMAND:
					{
						var uCmdType = (SysCommand)wParam;

						switch (uCmdType)
						{
							case SysCommand.SC_CLOSE:
								{
									// IConsoleService is responsible for closing the window
									_consoleInstance.Terminate();
									handled = true;
								}
								break;

							case SysCommand.SC_MINIMIZE:
								{
									_consoleInstance.IsVisible = false;
									User32Methods.ShowWindow(hwnd, ShowWindowCommands.SW_HIDE);
									handled = true;
								}
								break;
						}
					}
					break;

				case WM.SETCURSOR:
					{
						var wlParam = new Win32Param()
						{
							BaseValue = (uint)lParam
						};

						var wMouseMsg = (WM)wlParam.HIWord;

						switch (wMouseMsg)
						{
							case WM.LBUTTONUP:
							case WM.RBUTTONUP:
							case WM.MBUTTONUP:
								{
									Show(hwnd, wParam);
									//handled = true;
								}
								break;
						}
					}
					break;
			}

			return IntPtr.Zero;
		}

		protected virtual void Dispose(bool disposing)
		{
			_consoleHwndHost.Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void Show(IntPtr ownerWindowHandle, IntPtr processWindowHandle)
		{
			User32Methods.SetActiveWindow(ownerWindowHandle);
			User32Methods.SetForegroundWindow(processWindowHandle);
			User32Methods.SendMessage(ownerWindowHandle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			_consoleInstance.IsVisible = true;
		}
	}
}
