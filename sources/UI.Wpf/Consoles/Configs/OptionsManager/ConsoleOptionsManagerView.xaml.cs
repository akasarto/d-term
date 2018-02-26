using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Console options manager view.
	/// </summary>
	public partial class ConsoleOptionsManagerView : UserControl, IViewFor<IConsoleOptionsManagerViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionsManagerView()
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
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsoleOptionsManagerViewModel), typeof(ConsoleOptionsManagerView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IConsoleOptionsManagerViewModel ViewModel
		{
			get { return (IConsoleOptionsManagerViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsoleOptionsManagerViewModel)value; }
		}
	}
}
