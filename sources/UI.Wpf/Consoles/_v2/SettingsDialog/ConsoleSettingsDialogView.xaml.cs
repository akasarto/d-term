using ReactiveUI;
using System;
using System.Windows;
using System.Windows.Input;

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

				activator(this.Events().KeyUp.Subscribe(key =>
				{
					switch (key.Key)
					{
						case Key.Escape:
							CloseDialog();
							break;
					}
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

		private void Button_Click(object sender, RoutedEventArgs e) => CloseDialog();

		private void CloseDialog()
		{
			Close();
		}
	}
}
