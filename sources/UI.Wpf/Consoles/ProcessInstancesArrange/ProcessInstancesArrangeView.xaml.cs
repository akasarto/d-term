using ReactiveUI;
using System.Windows.Controls;
using System;

namespace UI.Wpf.Consoles
{
	public partial class ProcessInstancesArrangeView : UserControl, IViewFor<ProcessInstancesArrangeViewModel>
	{
		public ProcessInstancesArrangeView()
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

		public ProcessInstancesArrangeViewModel ViewModel
		{
			get => (ProcessInstancesArrangeViewModel)DataContext;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ProcessInstancesArrangeViewModel)value; }
		}
	}
}
