using AutoMapper;

namespace dTerm.UI.Wpf.Mappings
{
    public static class MapperFactory
    {
        public static IMapper Create()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ShellProcessesProfile>();
            });

            return config.CreateMapper();
        }
    }
}
