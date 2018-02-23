using System;
using Consoles.Core;

namespace UI.Wpf.Consoles
{
	public interface IConsoleOptionViewModel
	{
		Guid Id { get; set; }
		bool IsSupported { get; set; }
		string Name { get; set; }
		int OrderIndex { get; set; }
		string PicturePath { get; set; }
		ProcessBasePath ProcessBasePath { get; set; }
		string ProcessBasePathDescription { get; set; }
		string ProcessExecutableName { get; set; }
		string ProcessStartupArgs { get; set; }
		DateTime UTCCreation { get; set; }
	}
}