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
					viewModel.Initialize(consolesContainer);
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
	}
}
