using ReactiveUI;
using System;
using System.Windows.Controls;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleOptionsListView : UserControl, IViewFor<ConsoleOptionsListViewModel>
	{
		public ConsoleOptionsListView()
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

		public ConsoleOptionsListViewModel ViewModel
		{
			get => (ConsoleOptionsListViewModel)DataContext;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ConsoleOptionsListViewModel)value; }
		}
	}
}
