using Processes.Core;
using Processes.SystemDiagnostics;
using Splat;
using System;
using System.Diagnostics;
using System.IO;

namespace UI.Wpf.Processes
{
	//
	public interface IProcessInstanceFactory
	{
		bool CanCreate(ProcessBasePath processBasePath, string processExecutableName);
		IProcess Create(IProcessViewModel processViewModel, bool runAsAdmin = false);
	}

	//
	public class SystemProcessFactory : IProcessInstanceFactory
	{
		private readonly IProcessPathBuilder _processPathBuilder;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SystemProcessFactory(IProcessPathBuilder processPathBuilder = null)
		{
			_processPathBuilder = processPathBuilder ?? Locator.CurrentMutable.GetService<IProcessPathBuilder>();
		}

		public bool CanCreate(ProcessBasePath processBasePath, string processExecutableName)
		{
			var path = _processPathBuilder.Build(processBasePath, processExecutableName);

			return !string.IsNullOrWhiteSpace(path) && new FileInfo(path).Exists;
		}

		public IProcess Create(IProcessViewModel consoleOptionViewModel, bool runAsAdmin = false)
		{
			consoleOptionViewModel = consoleOptionViewModel ?? throw new ArgumentNullException(nameof(consoleOptionViewModel), nameof(SystemProcessFactory));

			if (CanCreate(consoleOptionViewModel.ProcessBasePath, consoleOptionViewModel.ProcessExecutableName))
			{
				var fullPath = _processPathBuilder.Build(consoleOptionViewModel.ProcessBasePath, consoleOptionViewModel.ProcessExecutableName);

				var processStartInfo = new ProcessStartInfo(fullPath)
				{
					UseShellExecute = true,
					WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
					Arguments = consoleOptionViewModel.ProcessStartupArgs,
					Verb = runAsAdmin ? "runas" : string.Empty
				};

				return new SystemProcess(processStartInfo);
			}

			return null;
		}
	}
}
