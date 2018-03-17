using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	public partial class TransparencyManagerPanelView : UserControl, IViewFor<ITransparencyManagerPanelViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public TransparencyManagerPanelView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ITransparencyManagerPanelViewModel), typeof(TransparencyManagerPanelView), new PropertyMetadata(null));

		public ITransparencyManagerPanelViewModel ViewModel
		{
			get { return (ITransparencyManagerPanelViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ITransparencyManagerPanelViewModel)value; }
		}
	}
}
