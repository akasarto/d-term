using ReactiveUI;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Consoles manager view.
	/// </summary>
	public partial class ConsolesManagerView : UserControl, IViewFor<IConsolesManagerViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesManagerView()
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
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsolesManagerViewModel), typeof(ConsolesManagerView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IConsolesManagerViewModel ViewModel
		{
			get { return (IConsolesManagerViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsolesManagerViewModel)value; }
		}
	}
}
