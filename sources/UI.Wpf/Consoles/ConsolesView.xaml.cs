using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	public partial class ConsolesView : UserControl, IViewFor<IConsolesViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadOptionsCommand).SelectMany(x => x.Execute()).Subscribe());
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsolesViewModel), typeof(ConsolesView), new PropertyMetadata(null));

		public IConsolesViewModel ViewModel
		{
			get { return (IConsolesViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsolesViewModel)value; }
		}
	}
}
