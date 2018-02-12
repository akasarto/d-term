using Dragablz.Dockablz;
using System.Collections.Specialized;
using System.Windows.Controls;
using ReactiveUI;
using System;

namespace UI.Wpf.Consoles
{
	public partial class ConsolesWorkspaceView : UserControl, IViewFor<ConsolesWorkspaceViewModel>
	{
		public ConsolesWorkspaceView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(x => x.ViewModel).Subscribe(viewModel =>
				{
					viewModel.Initialize();
				}));
			});
		}

		public ConsolesWorkspaceViewModel ViewModel
		{
			get => (ConsolesWorkspaceViewModel)DataContext;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ConsolesWorkspaceViewModel)value; }
		}

		private void ConsoleInstances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Layout.TileFloatingItemsCommand.Execute(null, consolesContainer);
		}
	}
}
