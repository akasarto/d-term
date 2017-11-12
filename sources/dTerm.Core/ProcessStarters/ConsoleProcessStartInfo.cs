using dTerm.Core.PathBuilders;
using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.ProcessStarters
{
	public class ConsoleProcessStartInfo
	{
		private bool _canStart;
		private string _arguments;
		private IPathBuilder _pathBuilder;

		public ConsoleProcessStartInfo(IPathBuilder pathBuilder, string arguments = null)
		{
			_pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder), nameof(ConsoleProcessStartInfo));
			_arguments = arguments;

			_canStart = new FileInfo(_pathBuilder.Build()).Exists;
		}

		public bool CanStart => _canStart;

		public static implicit operator ProcessStartInfo(ConsoleProcessStartInfo consoleInfo)
		{
			var processStartInfo = new ProcessStartInfo(consoleInfo._pathBuilder.Build())
			{
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = consoleInfo._arguments
			};

			return processStartInfo;
		}
	}
}
