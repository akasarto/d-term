using MaterialDesignThemes.Wpf;
using System;

namespace dTerm.UI.Wpf.Views
{
    public partial class TerminalShellViewModel : BaseViewModel
    {
        private PackIconKind _icon;
        private int _orderIndex;
        private string _name;
        private string _processStartupArgs;

        public Guid Id { get; init; }

        public PackIconKind Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public int OrderIndex
        {
            get => _orderIndex;
            set => SetProperty(ref _orderIndex, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string ProcessExecutablePath { get; set; }

        public string ProcessStartupArgs
        {
            get => _processStartupArgs;
            set => SetProperty(ref _processStartupArgs, value);
        }
    }
}
