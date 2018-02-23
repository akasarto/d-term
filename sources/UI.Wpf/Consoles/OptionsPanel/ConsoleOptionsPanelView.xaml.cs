using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleOptionsPanelView : UserControl, IViewFor<IConsoleOptionsPanelViewModel>
	{
		public ConsoleOptionsPanelView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.DataContext).BindTo(this, @this => @this.ViewModel));
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadOptionsCommand).SelectMany(x => x.Execute()).Subscribe());
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsoleOptionsPanelViewModel), typeof(ConsoleOptionsPanelView), new PropertyMetadata(null));

		public IConsoleOptionsPanelViewModel ViewModel
		{
			get { return (IConsoleOptionsPanelViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsoleOptionsPanelViewModel)value; }
		}
	}
}
