using ReactiveUI;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Reactive.Linq;
using System.Windows.Navigation;
using System.Diagnostics;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleView : UserControl, IViewFor<IConsoleOptionViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(Observable.FromEventPattern<RequestNavigateEventHandler, RequestNavigateEventArgs>(
					handler => iconSampleLink.RequestNavigate += handler,
					handler => iconSampleLink.RequestNavigate -= handler)
					.Subscribe(@event =>
					{
						Process.Start(new ProcessStartInfo(@event.EventArgs.Uri.AbsoluteUri));
						@event.EventArgs.Handled = true;
					}));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsoleOptionViewModel), typeof(ConsoleView), new PropertyMetadata(null));

		public IConsoleOptionViewModel ViewModel
		{
			get { return (IConsoleOptionViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsoleOptionViewModel)value; }
		}
	}
}
