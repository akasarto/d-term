using AutoMapper;
using Processes.Core;
using Humanizer;
using Splat;
using UI.Wpf.Processes;

namespace UI.Wpf.Mappings
{
	/// <summary>
	/// Processes map definitions profile.
	/// </summary>
	public class MapperProfileProcesses : Profile
	{
		//
		private readonly IProcessFactory _processService;

		/// <summary>
		/// constructor method.
		/// </summary>
		public MapperProfileProcesses(IProcessFactory processService = null)
		{
			_processService = processService ?? Locator.CurrentMutable.GetService<IProcessFactory>();

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
			CreateMap<ProcessEntity, IProcessViewModel>().ConstructUsing(source => _locator.GetService<IProcessViewModel>()).AfterMap((source, dest) =>
			{
				dest.ProcessBasePathDescription = source.ProcessBasePath.Humanize();
			});
			CreateMap<IProcessViewModel, ProcessEntity>();

			//
			CreateMap<ProcessEntity, IProcessOptionViewModel>().ConstructUsing(source => _locator.GetService<IProcessOptionViewModel>()).AfterMap((source, dest) =>
			{
				dest.IsSupported = _processService.CanCreate(source.ProcessBasePath, source.ProcessExecutableName);
			});
			CreateMap<IProcessOptionViewModel, ProcessEntity>();
		}
	}
}
