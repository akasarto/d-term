using Processes.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Processes.SystemDiagnostics
{
	public class ProcessesTracker : IProcessesTracker
	{
		private List<int> _processIds = new List<int>();

		public void Dispose()
		{
			KillAll();
		}

		public void KillAll()
		{
			if (_processIds.Count <= 0)
			{
				return;
			}

			using (var process = new Process())
			{
				var pids = _processIds.Distinct().ToList();

				var args = $"{string.Join("", pids.Select(pid => $"/PID {pid} "))} /T /F";

				process.StartInfo = new ProcessStartInfo()
				{
					FileName = "taskkill",
					ErrorDialog = false,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					Arguments = args
				};

				process.Start();

				var output = process.StandardOutput.ReadToEnd();
				var error = process.StandardError.ReadToEnd();

				process.WaitForExit();
			}
		}

		public void Track(int processId)
		{
			_processIds.Add(processId);
		}
	}
}
