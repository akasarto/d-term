using dTerm.Core.PathBuilders;
using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.ProcessStarters
{
	public class ConsoleProcessStartInfo
	{
		private bool? _canStart;
		private string _arguments;
		private IPathBuilder _pathBuilder;

		public ConsoleProcessStartInfo(IPathBuilder pathBuilder, string arguments = null)
		{
			_pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder), nameof(ConsoleProcessStartInfo));
			_arguments = arguments;

		}

		public bool CanStart
		{
			get
			{
				if (!_canStart.HasValue)
				{
					var path = _pathBuilder.Build();

					_canStart = string.IsNullOrWhiteSpace(path) ? false : new FileInfo(path).Exists;
				}

				return _canStart.Value;
			}
		}

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
