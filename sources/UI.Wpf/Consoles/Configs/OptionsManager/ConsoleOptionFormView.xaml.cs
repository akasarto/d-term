using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Console option form view.
	/// </summary>
	public partial class ConsoleOptionFormView : UserControl, IViewFor<IConsoleOptionFormViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionFormView()
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
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsoleOptionFormViewModel), typeof(ConsoleOptionFormView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IConsoleOptionFormViewModel ViewModel
		{
			get { return (IConsoleOptionFormViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsoleOptionFormViewModel)value; }
		}
	}
}
