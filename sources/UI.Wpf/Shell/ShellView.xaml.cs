using System;
using System.Windows;
using System.Windows.Interop;

namespace UI.Wpf.Shell
{
	public partial class ShellView : Window
	{
		private IntPtr _shellHandle;
		private ShellWndProc _shellWndProc;

		public ShellView(ShellViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;

			SourceInitialized += (object sender, EventArgs args) =>
			{
				_shellHandle = new WindowInteropHelper(this).Handle;
				_shellWndProc = new ShellWndProc(HwndSource.FromHwnd(_shellHandle));
			};
		}
	}
}
