using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	public partial class UtilitiesPanelView : UserControl, IViewFor<IUtilitiesPanelViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public UtilitiesPanelView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadUtilitiesCommand).SelectMany(x => x.Execute()).Subscribe());
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IUtilitiesPanelViewModel), typeof(UtilitiesPanelView), new PropertyMetadata(null));

		public IUtilitiesPanelViewModel ViewModel
		{
			get { return (IUtilitiesPanelViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IUtilitiesPanelViewModel)value; }
		}
	}
}
