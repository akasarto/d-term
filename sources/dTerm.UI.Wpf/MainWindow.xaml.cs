using dTerm.Infra.EfCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class MainWindow : Window
    {
        private readonly IServiceScope _serviceScope;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;

            _serviceScope = serviceProvider.CreateScope();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                using (var context = _serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    var consoles = context.Consoles.ToList();
                }

                using (var context = _serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    var consoles = context.Consoles.ToList();

                    Dispatcher.Invoke(() =>
                    {
                        loader.Visibility = Visibility.Collapsed;

                        Consoles.ItemsSource = consoles;
                    });
                }
            });
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (var context = _serviceScope.ServiceProvider.GetService<AppDbContext>())
            {
                var consoles = context.Consoles.ToList();
            }

            foreach (Window window in OwnedWindows)
            {
                window.Close();
            }

            _serviceScope.Dispose();
        }
    }
}
