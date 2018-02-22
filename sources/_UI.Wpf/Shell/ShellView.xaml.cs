using ReactiveUI;
using System;
using System.Windows;
using System.Windows.Interop;

namespace UI.Wpf.Shell
{
	public partial class ShellView : IViewFor<IShellViewModel>
	{
		private IntPtr _shellHandle;
		private ShellWndProc _shellWndProc;

		public ShellView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.Events().SourceInitialized.Subscribe(args =>
				{
					_shellHandle = new WindowInteropHelper(this).Handle;
					_shellWndProc = new ShellWndProc(HwndSource.FromHwnd(_shellHandle));
				}));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IShellViewModel), typeof(ShellView), new PropertyMetadata(null));

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
