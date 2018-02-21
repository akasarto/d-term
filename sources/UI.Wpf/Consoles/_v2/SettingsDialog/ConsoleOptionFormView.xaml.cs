using System.Diagnostics;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleOptionFormView : UserControl
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
	}
}
