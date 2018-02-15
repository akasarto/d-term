using Consoles.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Consoles.Processes
{
	public class ProcessTracker : IProcessTracker
	{
		private List<int> _processIds = new List<int>();

		public void Add(int processId)
		{
			_processIds.Add(processId);
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
	}
}
