using AutoMapper;
using dTerm.Core;
using dTerm.UI.Wpf.Views;

namespace dTerm.UI.Wpf.Mappings
{
    public class ShellProcessesProfile : Profile
    {
        public ShellProcessesProfile()
        {
            CreateMap<ProcessEntity, ShellProcessToolbarOptionButtonViewModel>();
            CreateMap<ProcessEntity, ShellProcessEditorViewModel>();
        }
    }
}
