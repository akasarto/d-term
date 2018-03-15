using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	public partial class MinimizedInstancesPanelView : UserControl, IViewFor<IMinimizedInstancesPanelViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public MinimizedInstancesPanelView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IMinimizedInstancesPanelViewModel), typeof(MinimizedInstancesPanelView), new PropertyMetadata(null));

		public IMinimizedInstancesPanelViewModel ViewModel
		{
			get { return (IMinimizedInstancesPanelViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IMinimizedInstancesPanelViewModel)value; }
		}
	}
}
