using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace dTerm.Core.ProcessTerminators
{
	public class ConsoleProcessKiller
	{
		private List<int> _consoleProcessIds = new List<int>();

		public void AddProcessId(int consoleProcessId) => _consoleProcessIds.Add(consoleProcessId);

		public void Execute()
		{
			if (_consoleProcessIds.Count <= 0)
			{
				throw new InvalidOperationException("No process ids provided.");
			}

			using (var process = new Process())
			{
				var pids = _consoleProcessIds.Distinct().ToList();

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

		public static ConsoleProcessKiller Create() => new ConsoleProcessKiller();
	}
}
