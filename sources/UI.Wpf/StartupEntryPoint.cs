using UI.Wpf.Shell;
using SimpleInjector;
using System;
using UI.Wpf.Mappings;
using System.Linq;

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
					if (args.Any(a => a.Equals("--verify")))
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
	}
}
