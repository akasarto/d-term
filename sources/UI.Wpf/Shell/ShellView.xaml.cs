using ReactiveUI;
using System.Reactive;
using System.Windows;
using UI.Wpf.Processes;

namespace UI.Wpf.Shell
{
	/// <summary>
	/// Shell view.
	/// </summary>
	public partial class ShellView : IActivatable
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellView(IShellViewModel shellViewModel)
		{
			InitializeComponent();

			DataContext = shellViewModel;

			this.WhenActivated(activator =>
			{
				activator(shellViewModel.Processes.OpenConfigsInteraction.RegisterHandler(context =>
				{
					var settingsView = new ConfigsView(context.Input)
					{
						Owner = Application.Current.MainWindow
					};

					settingsView.ShowDialog();

					context.SetOutput(Unit.Default);
				}));
			});
		}
	}
}
