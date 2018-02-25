using ReactiveUI;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Console configurations view.
	/// </summary>
	public partial class ConsoleConfigsView : UserControl, IViewFor<IConsoleConfigsViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleConfigsView()
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
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsoleConfigsViewModel), typeof(ConsoleConfigsView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IConsoleConfigsViewModel ViewModel
		{
			get { return (IConsoleConfigsViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsoleConfigsViewModel)value; }
		}
	}
}
