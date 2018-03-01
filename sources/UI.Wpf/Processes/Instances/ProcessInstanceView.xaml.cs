using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process intance view.
	/// </summary>
	public partial class ProcessInstanceView : IViewFor<IProcessInstanceViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstanceView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
			});
		}

		/// <summary>
		/// View model dependency property backing field.
		/// </summary>
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessInstanceViewModel), typeof(ProcessInstanceView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IProcessInstanceViewModel ViewModel
		{
			get { return (IProcessInstanceViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessInstanceViewModel)value; }
		}
	}
}
