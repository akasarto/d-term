using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace UI.Wpf.Processes
{
	[ViewContract("ProcessFormView")]
	public partial class ConsoleOptionView : UserControl, IViewFor<IProcessViewModel>
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

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessViewModel), typeof(ConsoleOptionView), new PropertyMetadata(null));

		public IProcessViewModel ViewModel
		{
			get { return (IProcessViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessViewModel)value; }
		}
	}
}
