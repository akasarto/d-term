using AutoMapper;

namespace UI.Wpf.Mappings
{
	public static class MapperInitializer
	{
		public static void Initialize(AppContainer startupContainer)
		{
			var consolesMapProfile = startupContainer.GetInstance<MapProfileConsoles>();
			var notebookMapProfile = startupContainer.GetInstance<MapProfileNotebook>();

			Mapper.Initialize(config =>
			{
				config.AddProfile(consolesMapProfile);
				config.AddProfile(notebookMapProfile);
			});
		}
	}
}
