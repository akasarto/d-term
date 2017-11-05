using dTerm.UI.Wpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace dTerm.UI.Wpf.Views
{
	public partial class ShellView
	{
		public ShellView(ShellViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}

		private void TabItem_Click(object sender, RoutedEventArgs e)
		{
			((TabItem)sender).IsSelected = true;
			e.Handled = true;
		}
	}
}
