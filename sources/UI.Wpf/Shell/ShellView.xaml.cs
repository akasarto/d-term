using System;
using System.Windows;
using System.Windows.Interop;

namespace UI.Wpf.Shell
{
	public partial class ShellView : Window
	{
		private ShellWndProc _wndProc = null;

		public ShellView(ShellViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
			SourceInitialized += (object sender, EventArgs args) =>
			{
				var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
				_wndProc = new ShellWndProc(hwndSource);
			};
		}
	}
}
