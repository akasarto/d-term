using AutoMapper;
using Consoles.Core;
using Consoles.Processes;
using ReactiveUI;
using System;
using System.Reactive;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionViewModel : BaseViewModel
	{
		private Guid _id;
		private int _index;
		private string _name;
		private string _iconPath;
		private PathBuilder _processPathBuilder;
		private string _processPathExeFilename;
		private string _processPathExeArgs;
		private DateTime _utcCreation;

		//
		private readonly IConsolesProcessService _consolesProcessService = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionViewModel(IConsolesProcessService consolesProcessService)
		{
			_consolesProcessService = consolesProcessService ?? throw new ArgumentNullException(nameof(consolesProcessService), nameof(ConsoleOptionViewModel));

			SetupCommands();
		}

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		public Guid Id
		{
			get => _id;
			set => this.RaiseAndSetIfChanged(ref _id, value);
		}

		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		public int Index
		{
			get => _index;
			set => this.RaiseAndSetIfChanged(ref _index, value);
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get => _name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		/// <summary>
		/// Gets or sets the icon path.
		/// </summary>
		public string IconPath
		{
			get => _iconPath;
			set => this.RaiseAndSetIfChanged(ref _iconPath, value);
		}

		/// <summary>
		/// Gets or sets the process path builder.
		/// </summary>
		public PathBuilder ProcessPathBuilder
		{
			get => _processPathBuilder;
			set => this.RaiseAndSetIfChanged(ref _processPathBuilder, value);
		}

		/// <summary>
		/// Gets or sets the process path executable file name.
		/// </summary>
		public string ProcessPathExeFilename
		{
			get => _processPathExeFilename;
			set => this.RaiseAndSetIfChanged(ref _processPathExeFilename, value);
		}

		/// <summary>
		/// Gets or sets the process path executable startup arguments.
		/// </summary>
		public string ProcessPathExeStartupArgs
		{
			get => _processPathExeArgs;
			set => this.RaiseAndSetIfChanged(ref _processPathExeArgs, value);
		}

		/// <summary>
		/// Gets or sets the UTC creation data and time.
		/// </summary>
		public DateTime UTCCreation
		{
			get => _utcCreation;
			set => this.RaiseAndSetIfChanged(ref _utcCreation, value);
		}

		/// <summary>
		/// Create a new console instance.
		/// </summary>
		public ReactiveCommand<ConsoleOptionViewModel, Unit> CreateInstanceCommand { get; protected set; }

		/// <summary>
		/// Wire up commands with their respective actions.
		/// <seealso cref="ProcessInstancesListViewModel"/>
		/// </summary>
		private void SetupCommands()
		{
			CreateInstanceCommand = ReactiveCommand.Create<ConsoleOptionViewModel>((consoleOptionViewModel) =>
			{
				var entity = Mapper.Map<ConsoleEntity>(this);

				var process = _consolesProcessService.Create(new ProcessDescriptor(entity)
				{
					StartupTimeoutInSeconds = 3
				});

				if (!process.IsStarted)
				{
					MessageBus.Current.SendMessage(new ConsoleErrorMessage()
					{
						Message = "Unable to start the new process."
					});

					return;
				}

				MessageBus.Current.SendMessage(new CreateProcessInstanceMessage(process));
			});
		}
	}
}
