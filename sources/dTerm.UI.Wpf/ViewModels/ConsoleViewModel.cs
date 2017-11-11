using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using System;
using System.Windows.Interop;
using WinApi.User32;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ConsoleViewModel : ObservableObject
	{
		private IntPtr _consoleViewHandle;
		private IConsoleProcess _consoleInstance;
		private ConsoleHwndHost _consoleHwndHost;

		public ConsoleViewModel(IConsoleProcess consoleInstance)
		{
			_consoleInstance = consoleInstance ?? throw new ArgumentNullException(nameof(consoleInstance), nameof(ConsoleViewModel));
		}

		public IntPtr ConsoleViewHandle
		{
			get => _consoleViewHandle;
			set
			{
				if (_consoleViewHandle != value)
				{
					Set(ref _consoleViewHandle, value);
					var hwndSource = HwndSource.FromHwnd(_consoleViewHandle);
					hwndSource.AddHook(new HwndSourceHook(WndProc));
				}
			}
		}

		public IConsoleProcess Instance => _consoleInstance;

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

		public void OnViewClosing()
		{
			_consoleInstance.Terminate();
			_consoleHwndHost.Dispose();
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;
			var viewHandle = _consoleViewHandle;
			var instanceHandle = _consoleInstance.ProcessMainWindowHandle;

			switch (message)
			{
				case WM.SETCURSOR:
					{
						if (wParam == instanceHandle)
						{
							var lwParam = new Win32LWParams()
							{
								Param = (uint)lParam
							};

							var wMouseMsg = (WM)lwParam.High;

							switch (wMouseMsg)
							{
								case WM.LBUTTONUP:
								case WM.RBUTTONUP:
								case WM.MBUTTONUP:
									{
										User32Methods.SetActiveWindow(viewHandle);
										User32Methods.SetForegroundWindow(instanceHandle);
										User32Methods.SetFocus(instanceHandle);
										User32Methods.SendMessage(viewHandle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
										handled = true;
									}
									break;
							}
						}
					}
					break;
			}

			return IntPtr.Zero;
		}
	}
}
