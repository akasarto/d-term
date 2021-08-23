using dTerm.Core;
using System.Windows;
using System.Windows.Controls;

namespace dTerm.UI.Wpf.Views
{
    public partial class ShellsToolbarOptionButton : UserControl
    {
        public ShellsToolbarOptionButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty EntityProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(ShellProcess),
            typeof(ShellsToolbarOptionButton),
            new PropertyMetadata(null)
        );

        public ShellProcess ViewModel
        {
            get { return (ShellProcess)GetValue(EntityProperty); }
            set
            {
                SetValue(EntityProperty, value);
                DataContext = value;
            }
        }

        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void OnChangeIconButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void OnExecuteButtonClick(object sender, RoutedEventArgs e)
        {
            var terminal = new TerminalWindow(ViewModel.ProcessExecutablePath);

            terminal.Show();
        }
    }
}
