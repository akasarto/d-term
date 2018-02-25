using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Console options panel view.
	/// </summary>
	public partial class ConsoleOptionsPanelView : UserControl, IViewFor<IConsolesPanelViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionsPanelView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadOptionsCommand).SelectMany(x => x.Execute()).Subscribe());
			});
		}

		/// <summary>
		/// View model dependency property backing field.
		/// </summary>
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsolesPanelViewModel), typeof(ConsoleOptionsPanelView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IConsolesPanelViewModel ViewModel
		{
			get { return (IConsolesPanelViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsolesPanelViewModel)value; }
		}
	}
}
