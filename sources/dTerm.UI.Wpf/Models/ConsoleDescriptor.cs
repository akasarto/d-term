using dTerm.Core;
using Humanizer;
using System.Diagnostics;

namespace dTerm.UI.Wpf.Models
{
	public class ConsoleDescriptor
	{
		private ConsoleType _consoleType;
		private ProcessStartInfo _processStartInfo;

		public ConsoleDescriptor(ConsoleType consoleType, ProcessStartInfo processStartInfo)
		{
			_consoleType = consoleType;
			_processStartInfo = processStartInfo;
		}

		public string ConsoleName => _consoleType.Humanize();

		public ConsoleType ConsoleType => _consoleType;

		public int DefautStartupTimeoutSeconds { get; set; } = 5;

		public int DisplayOrder { get; set; }

		public bool IsSupported => _processStartInfo?.PathExists() ?? false;

		public ProcessStartInfo ProcessStartInfo => _processStartInfo;
	}
}
