using ReactiveUI;

namespace UI.Wpf.Settings
{
	/// <summary>
	/// General settings view.
	/// </summary>
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
