using Processes.Core;
using Processes.SystemDiagnostics;
using Splat;
using System;
using System.Diagnostics;
using System.IO;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process factory interface.
	/// </summary>
	public interface IProcessFactory
	{
		bool CanCreate(ProcessBasePath processBasePath, string processExecutableName);
		IProcessInstance Create(IProcessViewModel IProcessViewModel);
	}

	/// <summary>
	/// App process factory implementation.
	/// </summary>
	public class ProcessFactory : IProcessFactory
	{
		private readonly IProcessTracker _processTracker = null;
		private readonly IProcessPathBuilder _processPathBuilder = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessFactory(IProcessTracker processTracker = null, IProcessPathBuilder processPathBuilder = null)
		{
			_processTracker = processTracker ?? Locator.CurrentMutable.GetService<IProcessTracker>();
			_processPathBuilder = processPathBuilder ?? Locator.CurrentMutable.GetService<IProcessPathBuilder>();
		}

		public bool CanCreate(ProcessBasePath processBasePath, string processExecutableName)
		{
			var path = _processPathBuilder.Build(processBasePath, processExecutableName);

			return !string.IsNullOrWhiteSpace(path) && new FileInfo(path).Exists;
		}

		public IProcessInstance Create(IProcessViewModel descriptor)
		{
			if (descriptor == null || !CanCreate(descriptor.ProcessBasePath, descriptor.ProcessExecutableName))
			{
				return null;
			}

			var fullPath = _processPathBuilder.Build(descriptor.ProcessBasePath, descriptor.ProcessExecutableName);

			var processStartInfo = new ProcessStartInfo(fullPath)
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				Arguments = descriptor.ProcessStartupArgs
			};

			var consoleInstance = new ProcessInstance(processStartInfo, 3);

			consoleInstance.Start();

			if (consoleInstance.IsStarted)
			{
				_processTracker.Track(consoleInstance.Id);
			}

			return consoleInstance;
		}
	}
}
