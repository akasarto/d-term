using AutoMapper;
using Processes.Core;
using Humanizer;
using Splat;
using UI.Wpf.Processes;
using System;
using System.Linq;
using System.Collections.Generic;

namespace UI.Wpf.Mappings
{
	/// <summary>
	/// Processes map definitions profile.
	/// </summary>
	public class MapperProfileProcesses : Profile
	{
		//
		private readonly IProcessInstanceFactory _processService;
		private readonly IProcessHostFactory _processHostFactory;

		/// <summary>
		/// constructor method.
		/// </summary>
		public MapperProfileProcesses(IProcessInstanceFactory processService = null, IProcessHostFactory processHostFactory = null)
		{
			_processService = processService ?? Locator.CurrentMutable.GetService<IProcessInstanceFactory>();
			_processHostFactory = processHostFactory ?? Locator.CurrentMutable.GetService<IProcessHostFactory>();

			SetupMaps();
		}

		/// <summary>
		/// Set all class mapping associations.
		/// </summary>
		private void SetupMaps()
		{
			var _locator = Locator.CurrentMutable;

			//
			CreateMap<ProcessEntity, IProcessViewModel>().ConstructUsing(source => _locator.GetService<IProcessViewModel>()).AfterMap((source, dest) =>
			{
				dest.IsSupported = _processService.CanCreate(source.ProcessBasePath, source.ProcessExecutableName);
				dest.ProcessBasePathDescription = source.ProcessBasePath.Humanize();

				dest.ProcessBasePathCollection = new List<EnumViewModel<ProcessBasePath>>();

				Enum.GetValues(typeof(ProcessBasePath)).Cast<ProcessBasePath>().ToList().ForEach(basePath =>
				{
					dest.ProcessBasePathCollection.Add(new EnumViewModel<ProcessBasePath>()
					{
						Description = basePath.Humanize(),
						Value = basePath
					});
				});
			});
			CreateMap<IProcessViewModel, ProcessEntity>();

			//
			CreateMap<IProcessInstance, IProcessInstanceViewModel>().ConstructUsing(
				source => new ProcessInstanceViewModel(source, _processHostFactory)
			);
		}
	}
}
