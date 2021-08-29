using AutoMapper;
using dTerm.Domain;
using dTerm.UI.Wpf.Mappings;
using dTerm.UI.Wpf.Stores;
using MaterialDesignThemes.Wpf;
using System.Windows;

namespace dTerm.UI.Wpf.Views
{
    public partial class TerminalShellToolbarOptionButton : BindableUserControl<TerminalShellToolbarOptionButtonViewModel>
    {
        private readonly ShellsStore _shellsStore = new ShellsStore();
        private readonly IMapper _mapper = MapperFactory.Create();

        public TerminalShellToolbarOptionButton()
        {
            InitializeComponent();
        }

        private async void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            var process = await _shellsStore.GetBytId(ViewModel.Id);

            var editor = new TerminalShellEditor()
            {
                ViewModel = _mapper.Map<TerminalShellEditorViewModel>(process)
            };

            var result = await DialogHost.Show(editor, DialogNames.Main);

            if (result is bool updated && updated)
            {
                var updatedProcess = _mapper.Map<ShellProcess>(editor.ViewModel);

                await _shellsStore.Update(updatedProcess);
            }
        }

        private async void OnChangeIconButtonClick(object sender, RoutedEventArgs e)
        {
            var browser = new IconBrowser();

            var result = await DialogHost.Show(browser, DialogNames.Main);

            if (result is bool updated && updated)
            {
                var process = await _shellsStore.GetBytId(ViewModel.Id);
                var selectedItem = (IconBrowserItemViewModel)browser.list.SelectedItem;

                process.Icon = selectedItem.Kind.ToString();

                await _shellsStore.Update(process);
            }
        }

        private async void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            var confirmationMessage = $"The shell process '{ViewModel.Name}' will be permanently deleted. Are you sure?";
            var confirmationResult = MessageBox.Show(confirmationMessage, "Shell Process Delete", MessageBoxButton.YesNo);

            if (confirmationResult == MessageBoxResult.Yes)
            {
                await _shellsStore.Delete(ViewModel.Id);
            }
        }

        private void OnExecuteButtonClick(object sender, RoutedEventArgs e)
        {
            var terminal = new TerminalWindow(ViewModel.ProcessExecutablePath)
            {
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = App.Current.MainWindow
            };

            terminal.Show();
        }
    }
}
