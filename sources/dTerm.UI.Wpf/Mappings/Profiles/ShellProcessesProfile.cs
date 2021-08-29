using AutoMapper;
using dTerm.Domain;
using dTerm.UI.Wpf.Views;

namespace dTerm.UI.Wpf.Mappings
{
    public class ShellProcessesProfile : Profile
    {
        public ShellProcessesProfile()
        {
            CreateMap<ShellProcess, TerminalShellToolbarOptionButtonViewModel>();
            CreateMap<ShellProcess, TerminalShellEditorViewModel>();

            CreateMap<TerminalShellEditorViewModel, ShellProcess>();
        }
    }
}
