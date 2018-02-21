using MahApps.Metro.Controls;
using ReactiveUI;
using System;
using System.Windows;
using System.Windows.Interop;

namespace UI.Wpf.Shell
{
	public partial class ShellView : MetroWindow, IViewFor<IShellViewModel>
	{
		private IntPtr _shellHandle;
		private ShellWndProc _shellWndProc;

		public ShellView()
		{
			InitializeComponent();

			SourceInitialized += (object sender, EventArgs args) =>
			{
				_shellHandle = new WindowInteropHelper(this).Handle;
				_shellWndProc = new ShellWndProc(HwndSource.FromHwnd(_shellHandle));
			};
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IShellViewModel), typeof(ShellView), new PropertyMetadata(0));

		public IShellViewModel ViewModel
		{
			get { return (IShellViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IShellViewModel)value; }
		}
	}
}
