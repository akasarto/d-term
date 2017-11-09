using dTerm.Core.Entities;
using dTerm.Core.Processes;
using dTerm.UI.Wpf.Infrastructure;
using System;

namespace dTerm.UI.Wpf.Models
{
	public class ConsoleInstance_ : ObservableObject
	{
		private string _name;
		private ConsoleType _consoleType;
		private IConsoleInstance _consoleProcess;
		private ProcessHwndHost _consoleProcessHost;

		public ConsoleInstance_(string name, ConsoleType consoleType, IConsoleInstance consoleProcess)
		{
			_name = name ?? throw new ArgumentNullException(nameof(name), nameof(ConsoleInstance_));
			_consoleProcess = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(ConsoleInstance_));
			_consoleType = consoleType;
		}

		public string Name
		{
			get => _name;
			set => Set(ref _name, value);
		}

		public IConsoleInstance ConsoleProcess => _consoleProcess;

		public ConsoleType ConsoleType
		{
			get => _consoleType;
			set => Set(ref _consoleType, value);
		}

		public ProcessHwndHost ConsoleProcessHost
		{
			get
			{
				if (_consoleProcessHost == null)
				{
					if (!_consoleProcess.ProcessIsStarted)
					{
						throw new InvalidOperationException($"[{nameof(ConsoleInstance_)}] Underlying process not running.");
					}

					_consoleProcessHost = new ProcessHwndHost(_consoleProcess);
				}

				return _consoleProcessHost;
			}
		}
	}
}
