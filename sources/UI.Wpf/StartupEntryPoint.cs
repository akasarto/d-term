using CommandLine;
using UI.Wpf.Infrastructure;
using UI.Wpf.Shell;
using SimpleInjector;
using System;
using System.Globalization;
using UI.Wpf.Mappings;
using Consoles.Core;

namespace UI.Wpf
{
	public class StartupEntryPoint
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var application = new App();
			var hasArguments = (args?.Length ?? 0) > 0;

			using (var container = new StartupContainer())
			{
				application.InitializeComponent();

				if (hasArguments)
				{
					var options = ParseArgs(args);

					if (options.Verify)
					{
						Console.WriteLine("Verifying...");
						container.Verify(VerificationOption.VerifyAndDiagnose);
						Console.WriteLine("Verification complete.");
					}

					return;
				}

				MapperInitializer.Initialize(container);

				var shellView = container.GetInstance<ShellView>();

				application.Run(shellView);
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
	}
}
