using Consoles.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionsPanelViewModel : ReactiveObject, IConsoleOptionsPanelViewModel
	{
		private ReactiveCommand<Unit, List<ConsoleOption>> _loadOptions;

		//
		private readonly IConsoleOptionsRepository _consoleOptionsRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		/// <param name="consoleOptionsRepository"></param>
		public ConsoleOptionsPanelViewModel(IConsoleOptionsRepository consoleOptionsRepository)
		{
			_consoleOptionsRepository = consoleOptionsRepository;

			LoadOptions = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _consoleOptionsRepository.GetAll();

				System.Threading.Thread.Sleep(5000);

				return Task.FromResult(items);
			}));

			LoadOptions.Subscribe(items =>
			{

			});
		}

		/// <summary>
		/// Load existing console options.
		/// </summary>
		public ReactiveCommand<Unit, List<ConsoleOption>> LoadOptions
		{
			get => _loadOptions;
			set => this.RaiseAndSetIfChanged(ref _loadOptions, value);
		}
	}
}
