using ReactiveUI;
using System.Diagnostics;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleOptionFormView : UserControl, IViewFor<ConsoleOptionFormViewModel>
	{
		public ConsoleOptionFormView()
		{
			InitializeComponent();

			iconSampleLink.RequestNavigate += (object sender, System.Windows.Navigation.RequestNavigateEventArgs args) =>
			{
				Process.Start(new ProcessStartInfo(args.Uri.AbsoluteUri));
				args.Handled = true;
			};
		}

		public ConsoleOptionFormViewModel ViewModel
		{
			get => (ConsoleOptionFormViewModel)DataContext;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ConsoleOptionFormViewModel)value; }
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

			if (openFileDialog.ShowDialog() == true)
			{
				var test = openFileDialog.FileName;
			}
		}
	}
}
