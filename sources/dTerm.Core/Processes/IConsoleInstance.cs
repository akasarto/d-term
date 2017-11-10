using dTerm.Core.Entities;
using System;

namespace dTerm.Core.Processes
{
	public interface IConsoleInstance
	{
		event EventHandler Started;
		event EventHandler Killed;

		bool IsRunning { get; }

		string Name { get; set; }

		int PorcessId { get; }

		IntPtr ProcessMainHandle { get; }

		IntPtr ProcessMainWindowHandle { get; }

		ConsoleType Type { get; set; }

		void Kill();

		void Start();

		void TransferOwnership(IntPtr ownerWindowHandle);
	}
}
