using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleInstancesPanelView : UserControl, IViewFor<IConsoleInstancesPanelViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleInstancesPanelView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
				//activator(this.WhenAnyValue(@this => @this.ViewModel.LoadOptionsCommand).SelectMany(x => x.Execute()).Subscribe());
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsoleInstancesPanelViewModel), typeof(ConsoleInstancesPanelView), new PropertyMetadata(null));

		public IConsoleInstancesPanelViewModel ViewModel
		{
			get { return (IConsoleInstancesPanelViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsoleInstancesPanelViewModel)value; }
		}
	}
}
