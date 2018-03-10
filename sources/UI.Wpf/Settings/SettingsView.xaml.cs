using ReactiveUI;

namespace UI.Wpf.Settings
{
	public partial class SettingsView : IActivatable
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public SettingsView(ISettingsViewModel settingsViewModel)
		{
			InitializeComponent();

			DataContext = settingsViewModel;

			this.WhenActivated(activator =>
			{
			});
		}
	}
}
