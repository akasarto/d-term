using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	public partial class MinimizedProcessesPanelView : UserControl, IViewFor<IMinimizedProcessesPanelViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public MinimizedProcessesPanelView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IMinimizedProcessesPanelViewModel), typeof(MinimizedProcessesPanelView), new PropertyMetadata(null));

		public IMinimizedProcessesPanelViewModel ViewModel
		{
			get { return (IMinimizedProcessesPanelViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IMinimizedProcessesPanelViewModel)value; }
		}
	}
}
