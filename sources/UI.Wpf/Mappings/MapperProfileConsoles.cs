using AutoMapper;
using Processes.Core;
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
		private readonly IProcessFactory _consoleProcessService;

		/// <summary>
		/// constructor method.
		/// </summary>
		public MapperProfileConsoles(IProcessFactory consoleProcessService = null)
		{
			_consoleProcessService = consoleProcessService ?? Locator.CurrentMutable.GetService<IProcessFactory>();

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
			CreateMap<ProcessEntity, IConsoleViewModel>().ConstructUsing(source => _locator.GetService<IConsoleViewModel>()).AfterMap((source, dest) =>
			{
				dest.IsSupported = _consoleProcessService.CanCreate(source.ProcessBasePath, source.ProcessExecutableName);
				dest.ProcessBasePathDescription = source.ProcessBasePath.Humanize();
			});
			CreateMap<IConsoleViewModel, ProcessEntity>();
		}
	}
}
