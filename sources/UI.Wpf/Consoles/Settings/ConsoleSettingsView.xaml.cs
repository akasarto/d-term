using ReactiveUI;
using System;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleSettingsView : Window, IViewFor<ConsoleSettingsViewModel>
	{
		public ConsoleSettingsView()
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

		public ConsoleSettingsViewModel ViewModel
		{
			get => (ConsoleSettingsViewModel)DataContext;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ConsoleSettingsViewModel)value; }
		}
	}
}
