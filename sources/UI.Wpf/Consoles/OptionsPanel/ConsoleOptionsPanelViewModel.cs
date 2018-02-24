﻿using AutoMapper;
using Consoles.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Console options panel view model interface.
	/// </summary>
	public interface IConsoleOptionsPanelViewModel
	{
		bool IsBusy { get; }
		ReactiveCommand<Unit, List<ConsoleOption>> LoadOptionsCommand { get; }
		IReactiveDerivedList<ConsoleOptionViewModel> Options { get; }
	}

	/// <summary>
	/// App console options panel view model implementation.
	/// <seealso cref="IConsoleOptionsPanelViewModel"/>
	/// </summary>
	public class ConsoleOptionsPanelViewModel : ReactiveObject, IConsoleOptionsPanelViewModel
	{
		//
		private readonly IConsoleOptionsRepository _consoleOptionsRepository;
		private readonly IReactiveList<ConsoleOption> _consoleOptionsSourceList;

		//
		private bool _isBusy;
		private ReactiveCommand<Unit, List<ConsoleOption>> _loadOptionsCommand;
		private IReactiveDerivedList<ConsoleOptionViewModel> _options;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionsPanelViewModel(IConsoleOptionsRepository consoleOptionsRepository)
		{
			_consoleOptionsRepository = consoleOptionsRepository ?? throw new ArgumentNullException(nameof(consoleOptionsRepository), nameof(ConsoleOptionsPanelViewModel));

			_consoleOptionsSourceList = new ReactiveList<ConsoleOption>();

			_options = _consoleOptionsSourceList.CreateDerivedCollection(
				selector: option => Mapper.Map<ConsoleOptionViewModel>(option)
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
		public ReactiveCommand<Unit, List<ConsoleOption>> LoadOptionsCommand => _loadOptionsCommand;

		/// <summary>
		/// Gets the current available console options.
		/// </summary>
		public IReactiveDerivedList<ConsoleOptionViewModel> Options => _options;

		/// <summary>
		/// Setup the load comand actions and observables.
		/// </summary>
		private void LoadOptionsCommandSetup()
		{
			_loadOptionsCommand = ReactiveCommand.CreateFromTask(async () => await Task.Run(() =>
			{
				var items = _consoleOptionsRepository.GetAll();

				System.Threading.Thread.Sleep(5000);

				return Task.FromResult(items);
			}));

			_loadOptionsCommand.IsExecuting.BindTo(this, @this => @this.IsBusy);

			_loadOptionsCommand.ThrownExceptions.Subscribe(@exception =>
			{
				// ToDo: Show message
			});

			_loadOptionsCommand.Subscribe(options =>
			{
				_consoleOptionsSourceList.Clear();
				_consoleOptionsSourceList.AddRange(options);
			});
		}
	}
}
