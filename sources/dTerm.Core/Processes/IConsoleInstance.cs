using dTerm.Core.Entities;
using System;

namespace dTerm.Core.Processes
{
	public interface IConsoleInstance
	{
		event EventHandler Started;
		event EventHandler Killed;

		string Name { get; set; }

		int PorcessId { get; }

		bool ProcessIsStarted { get; }

		IntPtr ProcessMainHandle { get; }

		IntPtr ProcessMainWindowHandle { get; }

		ConsoleType Type { get; set; }

		bool Kill();

		bool Start();

		void TransferOwnership(IntPtr ownerWindowHandle);
	}
}
