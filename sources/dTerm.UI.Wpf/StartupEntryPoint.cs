using CommandLine;
using dTerm.Consoles.Core;
using dTerm.Consoles.Processes;
using SimpleInjector;
using System;
using System.Globalization;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Shell;

namespace dTerm.UI.Wpf
{
	public class StartupEntryPoint
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

		private static StartupArgs ParseArgs(string[] rawArgs)
		{
			var result = new StartupArgs();
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
				.ParseArguments<StartupArgs>(rawArgs)
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
