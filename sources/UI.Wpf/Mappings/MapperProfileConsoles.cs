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
			CreateMap<ConsoleEntity, IConsoleViewModel>().ConstructUsing(source => _locator.GetService<IConsoleViewModel>()).AfterMap((source, dest) =>
			{
				dest.IsSupported = _consoleProcessService.CanCreate(source.ProcessBasePath, source.ProcessExecutableName);
				dest.ProcessBasePathDescription = source.ProcessBasePath.Humanize();
			});
			CreateMap<IConsoleViewModel, ConsoleEntity>();
		}
	}
}
