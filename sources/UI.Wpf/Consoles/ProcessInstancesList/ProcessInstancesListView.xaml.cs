using ReactiveUI;
using System;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	public partial class ProcessInstancesListView : UserControl, IViewFor<ProcessInstancesListViewModel>
	{
		public ProcessInstancesListView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(x => x.ViewModel).Subscribe(viewModel =>
				{
					viewModel.Initialize(consoleInstancesControl);
				}));
			});
		}

		public ProcessInstancesListViewModel ViewModel
		{
			get => (ProcessInstancesListViewModel)DataContext;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProcessInstancesListViewModel)value; }
		}
	}
}
