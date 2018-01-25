using System.Windows;
using UI.Wpf.ViewModels;

namespace UI.Wpf.Views
{
	public partial class ShellView : Window
	{
		public ShellView(ShellViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}
