﻿using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newConsole = new ConsoleWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ShowInTaskbar = false,
                Owner = this
            };

            newConsole.Show();

            /*
            var processStartInfo = new System.Diagnostics.ProcessStartInfo();
            processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            processStartInfo.FileName = @"dTermConsole.exe";

            _ = System.Diagnostics.Process.Start(processStartInfo);
            */
        }
    }
}
