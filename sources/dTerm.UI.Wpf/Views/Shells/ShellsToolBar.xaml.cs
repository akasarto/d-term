using dTerm.UI.Wpf.Stores;
using System.Windows;
using System.Windows.Controls;

namespace dTerm.UI.Wpf.Views
{
    public partial class ShellsToolBar : ToolBar
    {
        private readonly ShellProcessStore _shellStore = new();

        public ShellsToolBar()
        {
            InitializeComponent();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            list.ItemsSource = _shellStore.CreateCollection();

            await _shellStore.LoadButtonOptions();

            loader.Visibility = Visibility.Collapsed;
            list.Visibility = Visibility.Visible;
        }
    }
}
