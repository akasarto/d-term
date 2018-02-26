using AutoMapper;
using Consoles.Core;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Consoles panel view model interface.
	/// </summary>
	public interface IConsolesPanelViewViewModel
	{
		bool IsBusy { get; }
		ReactiveCommand<Unit, List<ConsoleEntity>> LoadOptionsCommand { get; }
		IReactiveDerivedList<IConsoleViewModel> Consoles { get; }
	}

	/// <summary>
	/// App consoles panel view model implementation.
	/// <seealso cref="IConsolesPanelViewViewModel"/>
	/// </summary>
	public class ConsolesPanelViewViewModel : ReactiveObject, IConsolesPanelViewViewModel
	{
		//
		private readonly IConsoleOptionsRepository _consoleOptionsRepository;

		//
		private bool _isBusy;
		private ReactiveCommand<Unit, List<ConsoleEntity>> _loadOptionsCommand;
		private IReactiveDerivedList<IConsoleViewModel> _consoles;
		private IReactiveList<ConsoleEntity> _processes;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesPanelViewViewModel(IConsoleOptionsRepository consoleOptionsRepository = null)
		{
			_consoleOptionsRepository = consoleOptionsRepository ?? Locator.CurrentMutable.GetService<IConsoleOptionsRepository>();

			_processes = new ReactiveList<ConsoleEntity>();

			_consoles = _processes.CreateDerivedCollection(
				selector: option => Mapper.Map<ConsoleViewModel>(option)
			);

			LoadOptionsCommandSetup();
		}

		/// <summary>
		/// Gets or sets the loading status.
		/// </summary>
		public bool IsBusy
		{
			get => _isBusy;
			set => this.RaiseAndSetIfChanged(ref _isBusy, value);
		}

		/// <summary>
		/// Gets the load options command instance.
		/// </summary>
		public ReactiveCommand<Unit, List<ConsoleEntity>> LoadOptionsCommand => _loadOptionsCommand;

		/// <summary>
		/// Gets the current available console options.
		/// </summary>
		public IReactiveDerivedList<IConsoleViewModel> Consoles => _consoles;

		/// <summary>
		/// Setup the load comand actions and observables.
		/// </summary>
		private void LoadOptionsCommandSetup()
		{
			_loadOptionsCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _consoleOptionsRepository.GetAll();

				return Task.FromResult(items);
			}));

			_loadOptionsCommand.IsExecuting.BindTo(this, @this => @this.IsBusy);

			_loadOptionsCommand.ThrownExceptions.Subscribe(@exception =>
			{
				// ToDo: Show message
			});

			_loadOptionsCommand.Subscribe(options =>
			{
				_processes.Clear();
				_processes.AddRange(options);
			});
		}
	}
}
