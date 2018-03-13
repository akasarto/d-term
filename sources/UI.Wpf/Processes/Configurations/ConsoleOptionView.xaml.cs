using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace UI.Wpf.Processes
{
	public partial class ConsoleOptionView : UserControl, IViewFor<IConsoleOptionViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionView()
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

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsoleOptionViewModel), typeof(ConsoleOptionView), new PropertyMetadata(null));

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
