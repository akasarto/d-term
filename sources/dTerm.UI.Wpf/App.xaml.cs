﻿using dTerm.Infra.EfCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class App : Application
    {
        public App()
        {
            var defaultCultureInfo = new CultureInfo("en-US");

            CultureInfo.DefaultThreadCurrentCulture = defaultCultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCultureInfo;

            AppDomain.CurrentDomain.UnhandledException += AppGlobalExceptionsHandler;
        }

        private void AppGlobalExceptionsHandler(object sender, UnhandledExceptionEventArgs eventArgs)
        {
        }

        private void AppStartup(object sender, StartupEventArgs e)
        {
            InitData();
        }

        private static void InitData()
        {
            using var context = new AppDbContext();
            context.Database.Migrate();
        }
    }
}
