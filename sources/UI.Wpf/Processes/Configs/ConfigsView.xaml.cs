using ReactiveUI;

namespace UI.Wpf.Processes
{
	public partial class ConfigsView : IActivatable
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConfigsView(IConfigsViewModel settingsViewModel)
		{
			InitializeComponent();

			DataContext = settingsViewModel;

			this.WhenActivated(activator =>
			{
			});
		}
	}
}
