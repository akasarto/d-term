using Dragablz.Dockablz;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	public partial class ConsolesAreaView : UserControl
	{
		public ConsolesAreaView()
		{
			InitializeComponent();
		}

		private void ConsoleInstances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Layout.TileFloatingItemsCommand.Execute(null, consolesContainer);
		}
	}
}
