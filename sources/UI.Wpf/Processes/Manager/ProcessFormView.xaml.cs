using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process form view.
	/// </summary>
	public partial class ProcessFormView : UserControl, IViewFor<IProcessFormViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessFormView()
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
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessFormViewModel), typeof(ProcessFormView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IProcessFormViewModel ViewModel
		{
			get { return (IProcessFormViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessFormViewModel)value; }
		}
	}
}
