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

		/*
		public ProcessHwndHost ProcessHwndHost
		{
			get
			{
				if (_processHwndHost == null)
				{
					_dTermProcess.Start();

					if (!_dTermProcess.IsStarted)
					{
						//TODO: Log
						return null;
					}

					_processHwndHost = new ProcessHwndHost(_dTermProcess);
				}

				return _processHwndHost;
			}
		}
		*/
	}
}
