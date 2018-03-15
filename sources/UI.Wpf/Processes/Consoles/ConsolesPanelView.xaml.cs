using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	public partial class ConsolesPanelView : UserControl, IViewFor<IConsolesPanelViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesPanelView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadOptionsCommand).SelectMany(x => x.Execute()).Subscribe());
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsolesPanelViewModel), typeof(ConsolesPanelView), new PropertyMetadata(null));

		public IConsolesPanelViewModel ViewModel
		{
			get { return (IConsolesPanelViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsolesPanelViewModel)value; }
		}
	}
}
