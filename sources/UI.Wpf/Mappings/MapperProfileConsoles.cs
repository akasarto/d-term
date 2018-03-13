﻿using AutoMapper;
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
		private readonly IProcessInstanceFactory _processFactory;

		/// <summary>
		/// constructor method.
		/// </summary>
		public MapperProfileConsoles(IProcessInstanceFactory processFactory = null)
		{
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IProcessInstanceFactory>();

			SetupMaps();
		}

		private void SetupMaps()
		{
			var _locator = Locator.CurrentMutable;

			CreateMap<IConsoleOptionViewModel, IConsoleOptionViewModel>().ConstructUsing(source => _locator.GetService<IConsoleOptionViewModel>());

			CreateMap<ProcessEntity, IConsoleOptionViewModel>().ConstructUsing(source => _locator.GetService<IConsoleOptionViewModel>()).AfterMap((source, dest) =>
			{
				if (string.IsNullOrWhiteSpace(source.PicturePath))
				{
					dest.PicturePath = Resources.ProcessPicturePathDefault;
				}

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

			CreateMap<IConsoleOptionViewModel, ProcessEntity>();

			CreateMap<IProcess, IProcessInstanceModel>().ConstructUsing(source => new ProcessInstanceModel(source));

			CreateMap<IConsoleOptionViewModel, IProcessInstanceModel>();
		}
	}
}
