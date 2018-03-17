using AutoMapper;
using Humanizer;
using Processes.Core;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.Wpf.Processes;
using UI.Wpf.Properties;

namespace UI.Wpf.Mappings
{
	public class MapperProfileConsoles : Profile
	{
		private readonly IProcessFactory _processFactory;

		/// <summary>
		/// constructor method.
		/// </summary>
		public MapperProfileConsoles(IProcessFactory processFactory = null)
		{
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IProcessFactory>();

			SetupMaps();
		}

		private void SetupMaps()
		{
			var _locator = Locator.CurrentMutable;

			CreateMap<IProcessViewModel, IProcessViewModel>().ConstructUsing(source => _locator.GetService<IProcessViewModel>());

			CreateMap<ProcessEntity, IProcessViewModel>().ConstructUsing(source => _locator.GetService<IProcessViewModel>()).AfterMap((source, dest) =>
			{
				if (string.IsNullOrWhiteSpace(source.PicturePath))
				{
					dest.PicturePath = Resources.ProcessPicturePathDefault;
				}

				dest.IsSupported = _processFactory.CanCreate(source.ProcessBasePath, source.ProcessExecutableName);
				dest.ProcessBasePathDescription = source.ProcessBasePath.Humanize();

				dest.ProcessTypeCollection = new List<EnumViewModel<ProcessType>>();
				dest.ProcessBasePathCollection = new List<EnumViewModel<ProcessBasePath>>();

				Enum.GetValues(typeof(ProcessType)).Cast<ProcessType>().ToList().ForEach(type =>
				{
					dest.ProcessTypeCollection.Add(new EnumViewModel<ProcessType>()
					{
						Description = type == ProcessType.None ? string.Empty : type.Humanize(),
						Value = type
					});
				});

				Enum.GetValues(typeof(ProcessBasePath)).Cast<ProcessBasePath>().ToList().ForEach(basePath =>
				{
					dest.ProcessBasePathCollection.Add(new EnumViewModel<ProcessBasePath>()
					{
						Description = basePath == ProcessBasePath.None ? string.Empty : basePath.Humanize(),
						Value = basePath
					});
				});
			});

			CreateMap<IProcessViewModel, ProcessEntity>();

			CreateMap<IProcess, IProcessInstanceViewModel>().ConstructUsing(source => new ProcessInstanceViewModel(source));

			CreateMap<IProcessViewModel, IProcessInstanceViewModel>();
		}
	}
}
