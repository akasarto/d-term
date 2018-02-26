using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Console entry form view.
	/// </summary>
	public partial class ConsoleFormView : UserControl, IViewFor<IConsoleFormViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleFormView()
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
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsoleFormViewModel), typeof(ConsoleFormView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IConsoleFormViewModel ViewModel
		{
			get { return (IConsoleFormViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsoleFormViewModel)value; }
		}
	}
}
