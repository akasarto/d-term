using ReactiveUI;
using Splat;
using UI.Wpf.Workspace;

namespace UI.Wpf
{
	public interface IShellViewModel
	{
		IShellScreen Screen { get; }
	}

	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		//
		private readonly IShellScreen _shellScreen;
		private readonly IWorkspaceViewModel _workspaceViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(IShellScreen shellScreen = null, IWorkspaceViewModel workspaceViewModel = null)
		{
			_shellScreen = shellScreen ?? Locator.CurrentMutable.GetService<IShellScreen>();
			_workspaceViewModel = workspaceViewModel ?? Locator.CurrentMutable.GetService<IWorkspaceViewModel>();

			_shellScreen.Router.Navigate.Execute(_workspaceViewModel);
		}

		public IShellScreen Screen => _shellScreen;
	}
}
