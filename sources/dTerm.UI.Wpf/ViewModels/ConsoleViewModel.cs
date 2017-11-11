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

		public bool IsClosing { get; private set; }

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
					var hwndSource = HwndSource.FromHwnd(_consoleViewHandle);
					hwndSource.AddHook(new HwndSourceHook(WndProc));
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

		public void OnViewClosing()
		{
			IsClosing = true;
			_consoleInstance.Terminate();
		}

		private void SetTitle()
		{
			Title = $"[{_consoleInstance.ProcessId}] {_consoleInstance.Name}";
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var message = (WM)msg;

			switch (message)
			{
				case WM.SETCURSOR:
					{
						var wlParam = new Win32Param()
						{
							BaseValue = (uint)lParam
						};

						var wMouseMsg = (WM)wlParam.HighWord;

						switch (wMouseMsg)
						{
							case WM.LBUTTONUP:
							case WM.RBUTTONUP:
							case WM.MBUTTONUP:
								{
									User32Methods.SetActiveWindow(hwnd);
									User32Methods.SetForegroundWindow(wParam);
									User32Methods.SendMessage(hwnd, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
									handled = true;
								}
								break;
						}
					}
					break;
			}

			return IntPtr.Zero;
		}

		public void Dispose(bool disposing)
		{
			_consoleHwndHost.Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
