using CommandLine;
using UI.Wpf.Infrastructure;
using UI.Wpf.Shell;
using SimpleInjector;
using System;
using System.Globalization;

namespace UI.Wpf
{
	public class StartupEntryPoint
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var app = new App();
			var container = new StartupContainer();
			var hasArguments = (args?.Length ?? 0) > 0;

			app.InitializeComponent();

			if (hasArguments)
			{
				var options = ParseArgs(args);

				if (options.Verify)
				{
					System.Console.WriteLine("Verifying...");
					container.Verify(VerificationOption.VerifyAndDiagnose);
					System.Console.WriteLine("Verification complete.");
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
					config.HelpWriter = System.Console.Error;
				}
			);

			parser
				.ParseArguments<StartupArgs>(rawArgs)
				.WithParsed(parsedArgs => result = parsedArgs);

			return result;
		}
	}
}
