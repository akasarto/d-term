using System.Windows.Controls;

namespace UI.Wpf.Notebook
{
	public partial class NoteView : UserControl
	{
		public NoteView()
		{
			InitializeComponent();
			DataContext = new NoteViewModel();
		}
	}
}
