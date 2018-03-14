using ReactiveUI;
using Splat;

namespace UI.Wpf.Processes
{
	//
	public interface IMinimizedInstancesPanelViewModel
	{
		IReactiveDerivedList<IInstanceViewModel> Instances { get; }
	}

	//
	public class MinimizedInstancesPanelViewModel : IMinimizedInstancesPanelViewModel
	{
		private readonly IInstancesManager _instancesManager;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MinimizedInstancesPanelViewModel(IInstancesManager instancesManager = null)
		{
			_instancesManager = instancesManager ?? Locator.CurrentMutable.GetService<IInstancesManager>();
		}

		public IReactiveDerivedList<IInstanceViewModel> Instances => _instancesManager.GetAllInstances();
	}
}
