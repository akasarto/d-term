using App.Consoles.Core;
using App.Consoles.Service;
using CommandLine;
using SimpleInjector;
using System;
using System.Globalization;
using UI.Wpf.Infrastructure;
using UI.Wpf.ViewModels;
using UI.Wpf.Views;

namespace UI.Wpf
{
	public class EntryPoint
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var app = new App();
			var container = CreateContainer();
			var hasArguments = (args?.Length ?? 0) > 0;

			app.InitializeComponent();

			if (hasArguments)
			{
				var options = ParseArgs(args);

				if (options.Verify)
				{
					Console.WriteLine("Verifying...");
					container.Verify(VerificationOption.VerifyAndDiagnose);
					Console.WriteLine("Verification complete.");
				}
			}
			else
			{
				var shellView = container.GetInstance<ShellView>();
				app.Run(shellView);
			}
		}

		private static StartupOptions ParseArgs(string[] rawArgs)
		{
			var result = new StartupOptions();
			var parser = new Parser(
				config =>
				{
					config.EnableDashDash = true;
					config.IgnoreUnknownArguments = true;
					config.ParsingCulture = CultureInfo.GetCultureInfo("en-US");
					config.HelpWriter = Console.Error;
				}
			);

			parser
				.ParseArguments<StartupOptions>(rawArgs)
				.WithParsed(parsedArgs => result = parsedArgs);

			return result;
		}

		private static Container CreateContainer()
		{
			var container = new Container();

			container.Register<ShellView>();
			container.Register<ShellViewModel>();

			container.Register<IConsoleProcessService, ConsoleProcessService>(Lifestyle.Singleton);

			return container;
		}
	}
}
