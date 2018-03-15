using Processes.Core;
using System;
using System.Diagnostics;

namespace Processes.SystemDiagnostics
{
	public class SystemProcess : Process, IProcess
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public SystemProcess(ProcessStartInfo processStartInfo)
		{
			processStartInfo = processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo), nameof(SystemProcess));

			StartInfo = processStartInfo;
			EnableRaisingEvents = true;
		}

		public bool Start(int startupTimeoutInSeconds = 3)
		{
			var processStopwatch = Stopwatch.StartNew();
			var processTimeoutMiliseconds = startupTimeoutInSeconds * 1000;

			var newProcessStarted = base.Start();

			if (newProcessStarted)
			{
				while (processStopwatch.ElapsedMilliseconds <= processTimeoutMiliseconds)
				{
					if (MainWindowHandle != IntPtr.Zero)
					{
						return true;
					}
				}

				Kill();
			}

			return false;
		}

#warning review
		//public new IntPtr MainWindowHandle
		//{
		//	get
		//	{
		//		Refresh();
		//		if (HasExited)
		//		{
		//			return IntPtr.Zero;
		//		}
		//		return base.MainWindowHandle;
		//	}
		//}
	}
}
