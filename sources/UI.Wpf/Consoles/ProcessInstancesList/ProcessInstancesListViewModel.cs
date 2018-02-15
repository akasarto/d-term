using ReactiveUI;
using System;

namespace UI.Wpf.Consoles
{
	public class ProcessInstancesListViewModel : BaseViewModel
	{
		//
		//private IInputElement _consolesControl = null;
		private ReactiveList<ConsoleIProcessnstanceViewModel> _consoleInstanceViewModels;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstancesListViewModel()
		{
			_consoleInstanceViewModels = new ReactiveList<ConsoleIProcessnstanceViewModel>()
			{
			};

			_consoleInstanceViewModels.Changed.Subscribe(instances =>
			{
				//Layout.TileFloatingItemsCommand.Execute(null, _consolesControl);
			});
		}

		/// <summary>
		/// Current consoles instances list
		/// </summary>
		public ReactiveList<ConsoleIProcessnstanceViewModel> Instances
		{
			get => _consoleInstanceViewModels;
			set => this.RaiseAndSetIfChanged(ref _consoleInstanceViewModels, value);
		}
	}
}
