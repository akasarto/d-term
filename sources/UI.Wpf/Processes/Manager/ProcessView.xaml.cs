using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

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
				//activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
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
