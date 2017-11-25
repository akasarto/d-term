using dTerm.Core;
using dTerm.Core.Events;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
using System;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ConsoleViewModel : BaseViewModel, IDisposable
	{
		private IntPtr _shellViewHandle;
		private IntPtr _consoleViewHandle;
		private IConsoleInstance _consoleInstance;
		private ConsoleHwndHost _consoleHwndHost;
		private string _viewTitle;

		public ConsoleViewModel(IntPtr shellViewHandle, IConsoleInstance consoleInstance)
		{
			_shellViewHandle = shellViewHandle != IntPtr.Zero ? shellViewHandle : throw new ArgumentOutOfRangeException(nameof(shellViewHandle), IntPtr.Zero, nameof(ConsoleViewModel));
			_consoleInstance = consoleInstance ?? throw new ArgumentNullException(nameof(consoleInstance), nameof(ConsoleViewModel));

			SetTitle();
		}

		public IntPtr ConsoleViewHandle => _consoleViewHandle;

		public IConsoleInstance ConsoleInstance => _consoleInstance;

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

		public string ViewTitle
		{
			get => _viewTitle;
			set => Set(ref _viewTitle, value);
		}

		public override void Init(IntPtr viewHandle)
		{
			_consoleViewHandle = viewHandle;

			DisableMaximizeButton();
			SetWindowMessagesHook();
		}

		private void SetTitle()
		{
			ViewTitle = $"[PID {_consoleInstance.ProcessId}] {_consoleInstance.Name}";
		}

		private void DisableMaximizeButton()
		{
			var newStyle = (WindowStyles)NativeMethods.GetWindowLongPtr(_consoleViewHandle, WindowLongFlags.GWL_STYLE);
			newStyle &= ~WindowStyles.WS_MAXIMIZEBOX;
			NativeMethods.SetWindowLongPtr(_consoleViewHandle, WindowLongFlags.GWL_STYLE, new IntPtr((long)newStyle));
		}

		private void SetWindowMessagesHook()
		{
			var hwndSource = HwndSource.FromHwnd(_consoleViewHandle);
			hwndSource.AddHook(new HwndSourceHook(WndProc));
			ShowWindow(_shellViewHandle, _consoleViewHandle);
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;

			switch (message)
			{
				case WM.APPViewHighlight:
					{
						ShowWindow(hwnd, wParam);
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
									_consoleInstance.Terminate();
									HideWindow(hwnd);
									handled = true;
								}
								break;

							case SysCommand.SC_MINIMIZE:
								{
									EventBus.Publish(new HideConsoleMessage(_consoleInstance));
									HideWindow(hwnd);
									handled = true;
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
			if (_consoleHwndHost != null)
			{
				_consoleHwndHost.Dispose();
				_consoleHwndHost = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void HideWindow(IntPtr ownerWindowHandle)
		{
			NativeMethods.ShowWindow(ownerWindowHandle, ShowWindowCommands.SW_HIDE);
			NativeMethods.SetActiveWindow(_shellViewHandle);
		}

		private void ShowWindow(IntPtr ownerWindowHandle, IntPtr processWindowHandle)
		{
			NativeMethods.SetForegroundWindow(processWindowHandle);
			NativeMethods.SetActiveWindow(ownerWindowHandle);
			NativeMethods.SendMessage(ownerWindowHandle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			NativeMethods.SendMessage(ownerWindowHandle, (uint)WM.NCPAINT, new IntPtr(1), IntPtr.Zero);

			EventBus.Publish(new ShowConsoleMessage(_consoleInstance));
		}
	}
}
