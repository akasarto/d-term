using dTerm.UI.Wpf.Stores;
using System.Windows;
using System.Windows.Controls;

namespace dTerm.UI.Wpf.Views
{
    public partial class TerminalShellToolBarAddButton : UserControl
    {
        private readonly ShellsStore _shellStore = new ShellsStore();

        public TerminalShellToolBarAddButton()
        {
            InitializeComponent();
        }

        private async void OnAddNewClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".exe",
                Filter = Domain.Localization.Content.Executable_Files_Exe,
                Title = Domain.Localization.Content.Locate_New_Shell_Process_File
            };

            var fileName = (fileDialog.ShowDialog() ?? false) ? fileDialog.FileName : string.Empty;

            await _shellStore.Create(fileName);
        }
    }
}
