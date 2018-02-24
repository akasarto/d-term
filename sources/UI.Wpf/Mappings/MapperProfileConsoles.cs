using System;
using AutoMapper;
using Consoles.Core;
using Humanizer;
using UI.Wpf.Consoles;

namespace UI.Wpf.Mappings
{
	/// <summary>
	/// Consoles map definitions profile.
	/// </summary>
	public class MapperProfileConsoles : Profile
	{
		//
		private readonly IConsoleProcessService _consoleProcessService;

		/// <summary>
		/// constructor method.
		/// </summary>
		public MapperProfileConsoles(IConsoleProcessService consoleProcessService)
		{
			_consoleProcessService = consoleProcessService ?? throw new ArgumentNullException(nameof(consoleProcessService), nameof(MapperProfileConsoles));

			SetupMaps();
		}

		/// <summary>
		/// Set all class mapping associations.
		/// </summary>
		private void SetupMaps()
		{
			CreateMap<ConsoleOption, ConsoleOptionViewModel>().AfterMap((source, dest) =>
			{
				dest.ProcessBasePathDescription = source.ProcessBasePath.Humanize();
				dest.IsSupported = _consoleProcessService.CanCreate(source.ProcessBasePath, source.ProcessExecutableName);
			});

			CreateMap<ConsoleOptionViewModel, ConsoleOption>();
		}
	}
}
