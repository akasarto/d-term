using ReactiveUI;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Reactive.Linq;
using System.Windows.Navigation;
using System.Diagnostics;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process view.
	/// <seealso cref="IProcessViewModel"/>
	/// </summary>
	public partial class ProcessView : UserControl, IViewFor<IProcessViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessView()
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

		/// <summary>
		/// View model dependency property backing field.
		/// </summary>
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessViewModel), typeof(ProcessView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IProcessViewModel ViewModel
		{
			get { return (IProcessViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessViewModel)value; }
		}
	}
}
