using Dragablz;
using System.Windows.Controls;

namespace UI.Wpf.Notebook
{
	public partial class NotesListView : UserControl
	{
		private object[] _noteItems;

		public NotesListView()
		{
			InitializeComponent();

			AddHandler(DragablzItem.DragStarted, new DragablzDragStartedEventHandler(ItemDragStarted), true);
			AddHandler(DragablzItem.DragCompleted, new DragablzDragCompletedEventHandler(ItemDragCompleted), true);
		}

		private void ItemDragStarted(object sender, DragablzDragStartedEventArgs e)
		{
			var item = e.DragablzItem.DataContext;

			System.Diagnostics.Trace.WriteLine($"User started to drag item: {item}.");
		}

		private void ItemDragCompleted(object sender, DragablzDragCompletedEventArgs e)
		{
			var item = e.DragablzItem.DataContext;
			System.Diagnostics.Trace.WriteLine($"User finished dragging item: {item}.");

			if (_noteItems == null) return;

			System.Diagnostics.Trace.Write("Order is now: ");
			foreach (var i in _noteItems)
			{
				System.Diagnostics.Trace.Write(i + " ");
			}
			System.Diagnostics.Trace.WriteLine("");
		}

		private void StackPositionMonitor_OnOrderChanged(object sender, OrderChangedEventArgs e)
		{
			_noteItems = e.NewOrder;
		}
	}
}
