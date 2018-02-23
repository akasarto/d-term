using Consoles.Core;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionsPanelViewModel : ReactiveObject, IConsoleOptionsPanelViewModel
	{
		private readonly IConsoleOptionsListViewModel _consoleOptionsListViewModel;
		private readonly IConsoleOptionsRepository _consoleOptionsRepository;

		private ReactiveCommand<Unit, IReactiveList<ConsoleOption>> _loadOptionsCommand;
		private IReactiveList<ConsoleOption> _options;
		private bool _isBusy;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionsPanelViewModel(IConsoleOptionsListViewModel consoleOptionsListViewModel, IConsoleOptionsRepository consoleOptionsRepository)
		{
			_consoleOptionsRepository = consoleOptionsRepository ?? throw new ArgumentNullException(nameof(consoleOptionsRepository), nameof(ConsoleOptionsPanelViewModel));
			_consoleOptionsListViewModel = consoleOptionsListViewModel ?? throw new ArgumentNullException(nameof(consoleOptionsListViewModel), nameof(ConsoleOptionsPanelViewModel));

			LoadOptionsCommandSetup();
		}

		public IConsoleOptionsListViewModel ConsoleOptionsListViewModel => _consoleOptionsListViewModel;


		public ReactiveCommand<Unit, IReactiveList<ConsoleOption>> LoadOptionsCommand
		{
			get => _loadOptionsCommand;
			set => this.RaiseAndSetIfChanged(ref _loadOptionsCommand, value);
		}

		public bool IsBusy
		{
			get => _isBusy;
			set => this.RaiseAndSetIfChanged(ref _isBusy, value);
		}

		private void LoadOptionsCommandSetup()
		{
			LoadOptionsCommand = ReactiveCommand.CreateFromTask<IReactiveList<ConsoleOption>>(async () => await Task.Run(() =>
			{
				var items = _consoleOptionsRepository.GetAll();

				var result = new ReactiveList<ConsoleOption>(items);

				return Task.FromResult(result);
			}));

			LoadOptionsCommand.IsExecuting.BindTo(this, @this => @this.IsBusy);

			LoadOptionsCommand.ThrownExceptions.Subscribe(@exception =>
			{
				// ToDo: Show message
			});

			LoadOptionsCommand.Subscribe(options => _options = options);

			LoadOptionsCommand.InvokeCommand(ConsoleOptionsListViewModel.InitializeItemsCommand);
		}
	}
}
