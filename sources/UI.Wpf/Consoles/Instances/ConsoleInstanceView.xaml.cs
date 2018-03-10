using ReactiveUI;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleInstanceView : IViewFor<IConsoleInstanceViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleInstanceView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsoleInstanceViewModel), typeof(ConsoleInstanceView), new PropertyMetadata(null));

		public IConsoleInstanceViewModel ViewModel
		{
			get { return (IConsoleInstanceViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsoleInstanceViewModel)value; }
		}
	}
}
