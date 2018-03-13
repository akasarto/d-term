using Splat;

namespace UI.Wpf.Processes
{
	//
	public interface IConfigsViewModel
	{
		IConsolesTabViewModel ConsolesManagerViewModel { get; }
	}

	//
	public class ConfigsViewModel : IConfigsViewModel
	{
		private readonly IConsolesTabViewModel _consolesManagerViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConfigsViewModel(IConsolesTabViewModel consolesManagerViewModel = null)
		{
			_consolesManagerViewModel = consolesManagerViewModel ?? Locator.CurrentMutable.GetService<IConsolesTabViewModel>();
		}

		public IConsolesTabViewModel ConsolesManagerViewModel => _consolesManagerViewModel;
	}
}
