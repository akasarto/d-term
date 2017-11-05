using dTerm.UI.Wpf.ViewModels;
using System.Windows.Controls;

namespace dTerm.UI.Wpf.Views
{
	public partial class ConsoleView : UserControl
	{
		public ConsoleView(ConsoleViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}
