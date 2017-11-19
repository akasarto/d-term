using dTerm.Core;
using dTerm.Core.ProcessStarters;

namespace dTerm.UI.Wpf.Models
{
	public class ConsoleDescriptor
	{
		private ConsoleType _consoleType;
		private ConsoleProcessStartInfo _consoleProcessStartInfo;

		public ConsoleDescriptor(ConsoleType consoleType, ConsoleProcessStartInfo consoleProcessStartInfo)
		{
			_consoleType = consoleType;
			_consoleProcessStartInfo = consoleProcessStartInfo;
		}

		public ConsoleType ConsoleType => _consoleType;

#warning review
		public string Description => _consoleType.ToString();

		public int DisplayOrder { get; set; }

		public int OperationsTimeoutInSeconds { get; set; } = 3;

		public bool ProcessCanStart => _consoleProcessStartInfo.CanStart;

		public ConsoleProcessStartInfo ProcessStartInfo => _consoleProcessStartInfo;
	}
}
