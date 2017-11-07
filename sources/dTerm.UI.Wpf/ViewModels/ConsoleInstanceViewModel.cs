using System;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.Core.Entities;
using dTerm.Core.Processes;

namespace dTerm.UI.Wpf.ViewModels
{
	public class ConsoleInstanceViewModel : ObservableObject
	{
		private string _name;
		private ConsoleType _type;
		private ITermProcess _process;
		private ProcessHwndHost _processHwndHost;

		public ConsoleInstanceViewModel(string name, ConsoleType type, ITermProcess process)
		{
			_name = name ?? throw new ArgumentNullException(nameof(name), nameof(ConsoleInstanceViewModel));
			_process = process ?? throw new ArgumentNullException(nameof(process), nameof(ConsoleInstanceViewModel));
			_type = type;
		}

		public string Name
		{
			get => _name;
			set => Set(ref _name, value);
		}

		public ITermProcess Process => _process;

		public ConsoleType Type
		{
			get => _type;
			set => Set(ref _type, value);
		}

		public ProcessHwndHost ProcessHwndHost
		{
			get
			{
				if (_processHwndHost == null)
				{
					if (!_process.IsRunning)
					{
						throw new InvalidOperationException($"[{nameof(ConsoleInstanceViewModel)}] Underlying process not running.");
					}

					_processHwndHost = new ProcessHwndHost(_process);
				}

				return _processHwndHost;
			}
		}
	}
}
