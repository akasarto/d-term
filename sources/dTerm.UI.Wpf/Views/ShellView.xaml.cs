using dTerm.UI.Wpf.ViewModels;
using MahApps.Metro.Controls;

namespace dTerm.UI.Wpf.Views
{
	public partial class ShellView : MetroWindow
	{
		public ShellView(ShellViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}
