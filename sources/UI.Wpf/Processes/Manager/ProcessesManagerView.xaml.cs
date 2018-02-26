using ReactiveUI;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Processes manager view.
	/// </summary>
	public partial class ProcessesManagerView : UserControl, IViewFor<IProcessesManagerViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesManagerView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadProcessesReactiveCommand).SelectMany(x => x.Execute()).Subscribe());
			});
		}

		/// <summary>
		/// View model dependency property backing field.
		/// </summary>
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessesManagerViewModel), typeof(ProcessesManagerView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IProcessesManagerViewModel ViewModel
		{
			get { return (IProcessesManagerViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessesManagerViewModel)value; }
		}
	}
}
