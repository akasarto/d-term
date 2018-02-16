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
		private string _name;
		private int _orderIndex;
		private string _picturePath;
		private BasePath _processBasePath;
		private string _processExecutableName;
		private string _processStartupArgs;
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
		/// Gets the supported flag based on the base path and executable name.
		/// </summary>
		public bool IsSupported
		{
			get => _consolesProcessService.CanCreate(ProcessBasePath, ProcessExecutableName);
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
		/// Gets or sets the order index.
		/// </summary>
		public int OrderIndex
		{
			get => _orderIndex;
			set => this.RaiseAndSetIfChanged(ref _orderIndex, value);
		}

		/// <summary>
		/// Gets or sets the icon path.
		/// </summary>
		public string PicturePath
		{
			get => _picturePath;
			set => this.RaiseAndSetIfChanged(ref _picturePath, value);
		}

		/// <summary>
		/// Gets or sets the process base path.
		/// </summary>
		public BasePath ProcessBasePath
		{
			get => _processBasePath;
			set => this.RaiseAndSetIfChanged(ref _processBasePath, value);
		}

		/// <summary>
		/// Gets or sets the process executable file name.
		/// </summary>
		public string ProcessExecutableName
		{
			get => _processExecutableName;
			set => this.RaiseAndSetIfChanged(ref _processExecutableName, value);
		}

		/// <summary>
		/// Gets or sets the process executable startup arguments.
		/// </summary>
		public string ProcessStartupArgs
		{
			get => _processStartupArgs;
			set => this.RaiseAndSetIfChanged(ref _processStartupArgs, value);
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
