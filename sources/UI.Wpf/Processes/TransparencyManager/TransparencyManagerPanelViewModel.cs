using Splat;

namespace UI.Wpf.Processes
{
	public interface ITransparencyManagerPanelViewModel
	{
		byte AlphaLevel { get; set; }
	}

	public class TransparencyManagerPanelViewModel : ITransparencyManagerPanelViewModel
	{
		private readonly IAppState _appState;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public TransparencyManagerPanelViewModel(IAppState appState = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();
		}

		public byte AlphaLevel
		{
			get => _appState.AlphaLevel;
			set => _appState.AlphaLevel = value;
		}
	}
}
