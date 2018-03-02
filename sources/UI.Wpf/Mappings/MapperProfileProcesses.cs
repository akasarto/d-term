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
		private readonly IProcessFactory _processFactory;
		private readonly IProcessHostFactory _processHostFactory;

		/// <summary>
		/// constructor method.
		/// </summary>
		public MapperProfileProcesses(IProcessFactory processFactory = null, IProcessHostFactory processHostFactory = null)
		{
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IProcessFactory>();
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
			CreateMap<IProcessViewModel, ProcessEntity>();
			CreateMap<IProcessViewModel, IProcessViewModel>().ConstructUsing(source => _locator.GetService<IProcessViewModel>());
			CreateMap<ProcessEntity, IProcessViewModel>().ConstructUsing(source => _locator.GetService<IProcessViewModel>()).AfterMap((source, dest) =>
			{
				dest.IsSupported = _processFactory.CanCreate(source.ProcessBasePath, source.ProcessExecutableName);
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

			//
			CreateMap<IProcess, IProcessInstanceViewModel>().ConstructUsing(
				source => new ProcessInstanceViewModel(source, _processHostFactory)
			);

			//
			CreateMap<IProcessViewModel, IProcessInstanceViewModel>();
		}
	}
}
