using System.Windows.Controls;

namespace UI.Wpf.Notebook
{
	public partial class AddNoteView : UserControl
	{
		public AddNoteView()
		{
			InitializeComponent();
			DataContext = new AddNoteViewModel();
		}
	}
}
