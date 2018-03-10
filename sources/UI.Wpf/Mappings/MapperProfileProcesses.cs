using AutoMapper;
using Humanizer;
using Processes.Core;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.Wpf.Consoles;
using UI.Wpf.Properties;

namespace UI.Wpf.Mappings
{
	/// <summary>
	/// Processes map definitions profile.
	/// </summary>
	public class MapperProfileProcesses : Profile
	{
		//
		private readonly IConsolesFactory _processFactory;
		private readonly IConsoleHwndHostFactory _processHostFactory;

		/// <summary>
		/// constructor method.
		/// </summary>
		public MapperProfileProcesses(IConsolesFactory processFactory = null, IConsoleHwndHostFactory processHostFactory = null)
		{
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IConsolesFactory>();
			_processHostFactory = processHostFactory ?? Locator.CurrentMutable.GetService<IConsoleHwndHostFactory>();

			SetupMaps();
		}

		/// <summary>
		/// Set all class mapping associations.
		/// </summary>
		private void SetupMaps()
		{
			var _locator = Locator.CurrentMutable;

			//
			CreateMap<IConsoleViewModel, IConsoleViewModel>().ConstructUsing(source => _locator.GetService<IConsoleViewModel>());
			CreateMap<ProcessEntity, IConsoleViewModel>().ConstructUsing(source => _locator.GetService<IConsoleViewModel>()).AfterMap((source, dest) =>
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
			CreateMap<IConsoleViewModel, ProcessEntity>();

			//
			CreateMap<IProcess, IConsoleInstanceViewModel>().ConstructUsing(source => new ConsoleInstanceViewModel(source, _processHostFactory)).AfterMap((source, dest) =>
			{
				dest.IsConsole = source.MainWindowClassName.ToLower().Contains("consolewindowclass");
			});

			//
			CreateMap<IConsoleViewModel, IConsoleInstanceViewModel>();
		}
	}
}
