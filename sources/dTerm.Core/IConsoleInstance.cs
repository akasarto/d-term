using System;

namespace dTerm.Core
{
	public interface IConsoleInstance
	{
		event EventHandler ProcessTerminated;

		string Name { get; set; }

		int ProcessId { get; }

		IntPtr ProcessHandle { get; }

		IntPtr ProcessMainWindowHandle { get; }

		ConsoleType Type { get; }

		bool Initialize();

		void SetOwner(IntPtr ownerHandle);

		void ShowProcessView();

		void Terminate();
	}
}
