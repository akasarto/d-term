using AutoMapper;
using Consoles.Core;
using Humanizer;
using Splat;
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
		public MapperProfileConsoles(IConsoleProcessService consoleProcessService = null)
		{
			_consoleProcessService = consoleProcessService ?? Locator.CurrentMutable.GetService<IConsoleProcessService>();

			SetupMaps();
		}

		/// <summary>
		/// Set all class mapping associations.
		/// </summary>
		private void SetupMaps()
		{
			//
			var _locator = Locator.CurrentMutable;

			//
			CreateMap<ConsoleOption, IConsoleOptionViewModel>().ConstructUsing(source => _locator.GetService<IConsoleOptionViewModel>()).AfterMap((source, dest) =>
			{
				dest.IsSupported = _consoleProcessService.CanCreate(source.ProcessBasePath, source.ProcessExecutableName);
			});
			CreateMap<IConsoleOptionViewModel, ConsoleOption>();

			//
			CreateMap<ConsoleOption, IConsoleOptionFormViewModel>().ConstructUsing(source => _locator.GetService<IConsoleOptionFormViewModel>()).AfterMap((source, dest) =>
			{
				dest.ProcessBasePathDescription = source.ProcessBasePath.Humanize();
			});
			CreateMap<IConsoleOptionFormViewModel, ConsoleOption>();
		}
	}
}
