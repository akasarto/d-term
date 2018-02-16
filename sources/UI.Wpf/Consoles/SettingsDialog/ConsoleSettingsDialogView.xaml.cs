using ReactiveUI;
using System;
using System.Windows;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleSettingsDialogView : Window, IViewFor<ConsoleSettingsDialogViewModel>
	{
		public ConsoleSettingsDialogView()
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

		public ConsoleSettingsDialogViewModel ViewModel
		{
			get => (ConsoleSettingsDialogViewModel)DataContext;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ConsoleSettingsDialogViewModel)value; }
		}
	}
}
