using AutoMapper;
using Consoles.Core;
using Consoles.Processes;
using Notebook.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using UI.Wpf.Consoles;
using UI.Wpf.Notebook;

namespace UI.Wpf.Shell
{
	public class ShellViewModel : ReactiveObject
	{
		private readonly IMapper _mapper = null;
		private readonly INotebookRepository _notebookRepository = null;
		private readonly IConsoleProcessService _consoleProcessService = null;

		public ShellViewModel(IMapper mapper, INotebookRepository notebookRepository, IConsoleProcessService consoleProcessService)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), nameof(ShellViewModel));
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(ShellViewModel));
			_consoleProcessService = consoleProcessService ?? throw new ArgumentNullException(nameof(consoleProcessService), nameof(ShellViewModel));

			SetupCommands();
			SetupData();
		}

		public ReactiveList<NoteViewModel> Notes { get; set; }
		public ReactiveList<ConsoleInstanceViewModel> ConsoleInstances { get; set; }

		public ReactiveCommand CreateConsole { get; protected set; }

		private void CreateConsoleExecute()
		{
			var consoleProcess = _consoleProcessService.Create(new ProcessDescriptor()
			{
				FilePath = @"/cmd.exe",
				PathType = PathType.SystemPathVar
			});

			//consoleProcess.Start();

			var consoleInstanceViewModel = new ConsoleInstanceViewModel(consoleProcess)
			{
				Name = DateTime.Now.Millisecond.ToString()
			};

			ConsoleInstances.Add(consoleInstanceViewModel);
		}

		private void SetupCommands()
		{
			CreateConsole = ReactiveCommand.Create(CreateConsoleExecute);
		}

		private void SetupData()
		{
			ConsoleInstances = new ReactiveList<ConsoleInstanceViewModel>();

			var notes = _notebookRepository.GetAll();
			var notesViewModel = _mapper.Map<List<NoteViewModel>>(notes);

			Notes = new ReactiveList<NoteViewModel>(notesViewModel);
		}
	}
}
