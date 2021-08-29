using MaterialDesignThemes.Wpf;
using System.Windows;

namespace dTerm.UI.Wpf.Views
{
    public partial class TerminalShellEditor : BindableUserControl<TerminalShellEditorViewModel>
    {
        private readonly TerminalShellEditorViewModelValidator _dataValidator = new();

        public TerminalShellEditor()
        {
            InitializeComponent();
        }

        private void OnCancelChanges(object sender, RoutedEventArgs e)
        {
            DialogHost.Close(DialogNames.Main, parameter: false);
        }

        private void OnSaveChanges(object sender, RoutedEventArgs e)
        {
            if (_dataValidator.Validate(ViewModel) is var result && result.IsValid)
            {
                DialogHost.Close(DialogNames.Main, true);

                return;
            }

            ViewModel.NotifyErrors(result.Errors);
        }
    }
}
