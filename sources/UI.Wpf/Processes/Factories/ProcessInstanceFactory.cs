using Processes.Core;
using Processes.SystemDiagnostics;
using Splat;
using System;
using System.Diagnostics;
using System.IO;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process instance factory interface.
	/// </summary>
	public interface IProcessInstanceFactory
	{
		bool CanCreate(ProcessBasePath processBasePath, string processExecutableName);
		IProcessInstance Create(IProcessViewModel IProcessViewModel);
	}

	/// <summary>
	/// App process instance factory implementation.
	/// </summary>
	public class ProcessInstanceFactory : IProcessInstanceFactory
	{
		//
		private readonly IProcessTracker _processTracker = null;
		private readonly IProcessPathBuilder _processPathBuilder = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstanceFactory(IProcessTracker processTracker = null, IProcessPathBuilder processPathBuilder = null)
		{
			_processTracker = processTracker ?? Locator.CurrentMutable.GetService<IProcessTracker>();
			_processPathBuilder = processPathBuilder ?? Locator.CurrentMutable.GetService<IProcessPathBuilder>();
		}

		/// <summary>
		/// Checks if the process instance can be created.
		/// </summary>
		/// <param name="processBasePath">The type of base folder path.</param>
		/// <param name="processExecutableName">The process executable file name.</param>
		/// <returns><c>True</c> if can be created, otherwise, <c>false</c>.</returns>
		public bool CanCreate(ProcessBasePath processBasePath, string processExecutableName)
		{
			var path = _processPathBuilder.Build(processBasePath, processExecutableName);

			return !string.IsNullOrWhiteSpace(path) && new FileInfo(path).Exists;
		}

		/// <summary>
		/// Create a new process instance.
		/// </summary>
		/// <param name="descriptor">The process view model.</param>
		/// <returns>An <see cref="IProcessInstance"/> instance.</returns>
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
