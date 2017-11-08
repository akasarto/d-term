using dTerm.Core.Entities;
using dTerm.Core.Processes;
using dTerm.UI.Wpf.Infrastructure;
using System;

namespace dTerm.UI.Wpf.Models
{
	public class ConsoleInstance : ObservableObject
	{
		private string _name;
		private ConsoleType _consoleType;
		private IConsoleProcess _consoleProcess;
		private ProcessHwndHost _consoleProcessHost;

		public ConsoleInstance(string name, ConsoleType consoleType, IConsoleProcess consoleProcess)
		{
			_name = name ?? throw new ArgumentNullException(nameof(name), nameof(ConsoleInstance));
			_consoleProcess = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(ConsoleInstance));
			_consoleType = consoleType;
		}

		public string Name
		{
			get => _name;
			set => Set(ref _name, value);
		}

		public IConsoleProcess ConsoleProcess => _consoleProcess;

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
					if (!_consoleProcess.IsRunning)
					{
						throw new InvalidOperationException($"[{nameof(ConsoleInstance)}] Underlying process not running.");
					}

					_consoleProcessHost = new ProcessHwndHost(_consoleProcess);
				}

				return _consoleProcessHost;
			}
		}
	}
}
