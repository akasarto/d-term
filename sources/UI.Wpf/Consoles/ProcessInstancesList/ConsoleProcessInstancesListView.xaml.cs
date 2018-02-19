using ReactiveUI;
using System;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleProcessInstancesListView : UserControl, IViewFor<ConsoleProcessInstancesListViewModel>
	{
		public ConsoleProcessInstancesListView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(x => x.ViewModel).Subscribe(viewModel =>
				{
					viewModel.Initialize(consoleInstancesControl);
				}));

				activator(this.Events().SizeChanged.Subscribe(eventArgs =>
				{
					ViewModel.ArrangeProcessInstances();
				}));
			});
		}

		public ConsoleProcessInstancesListViewModel ViewModel
		{
			get => (ConsoleProcessInstancesListViewModel)DataContext;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ConsoleProcessInstancesListViewModel)value; }
		}
	}
}
