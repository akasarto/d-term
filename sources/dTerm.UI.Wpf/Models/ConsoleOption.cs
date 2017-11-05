using dTerm.Core.Entities;
using dTerm.Core.Processes;
using dTerm.UI.Wpf.Infrastructure;
using Humanizer;
using System;
using System.Diagnostics;

namespace dTerm.UI.Wpf.Models
{
	public class ConsoleOption : ObservableObject, IConsoleOption
	{
		private ConsoleType _consoleType;
		private ProcessStartInfo _processStartInfo;

		public ConsoleOption(ConsoleType consoleType, ProcessStartInfo processStartInfo)
		{
			_consoleType = consoleType;
			_processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(ConsoleOption));
		}

		public ConsoleType ConsoleType => _consoleType;

		public string Description => _consoleType.Humanize();

		public int DisplayOrder { get; set; }

		public bool IsSupported => _processStartInfo?.PathExists() ?? false;

		public ProcessStartInfo ProcessStartInfo => _processStartInfo;
	}
}
