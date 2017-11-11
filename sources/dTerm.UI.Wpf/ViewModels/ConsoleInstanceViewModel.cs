using dTerm.Core;
using dTerm.UI.Wpf.Infrastructure;
using System;
using System.Windows.Interop;
using WinApi.User32;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ConsoleInstanceViewModel : ObservableObject
	{
		private IntPtr _consoleViewHandle;
		private IConsoleProcess _consoleProcess;
		private ConsoleHwndHost _consoleHwndHost;

		public ConsoleInstanceViewModel(IConsoleProcess consoleProcess)
		{
			_consoleProcess = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(ConsoleInstanceViewModel));
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
					var hwndSource = HwndSource.FromHwnd(_consoleViewHandle);
					hwndSource.AddHook(new HwndSourceHook(WndProc));
				}
			}
		}

		public IConsoleProcess Process => _consoleProcess;

		public ConsoleHwndHost ConsoleHwndHost
		{
			get
			{
				if (_consoleHwndHost == null)
				{
					_consoleHwndHost = new ConsoleHwndHost(_consoleProcess);
				}

				return _consoleHwndHost;
			}
		}

		public void OnViewClosing()
		{
			_consoleProcess.Terminate();
			_consoleHwndHost.Dispose();
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;
			var viewHandle = _consoleViewHandle;
			var instanceHandle = _consoleProcess.ProcessMainWindowHandle;

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
