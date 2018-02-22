using ReactiveUI;
using UI.Wpf.Consoles;

namespace UI.Wpf.Shell
{
	/// <summary>
	/// App main view.
	/// </summary>
	public partial class ShellView
	{
		public ShellView()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			xxx.ViewModel = new ConsoleOptionsPanelViewModel();
		}
	}
}
