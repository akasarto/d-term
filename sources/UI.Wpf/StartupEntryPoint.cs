using UI.Wpf.Shell;
using SimpleInjector;
using System;
using UI.Wpf.Mappings;
using System.Linq;
using MaterialDesignThemes.Wpf;

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

				application.DispatcherUnhandledException += (object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs exceptionArgs) =>
				{
					//ToDo: Log exceptions (v3 milestone)
					var notifier = container.GetInstance<ISnackbarMessageQueue>();
					notifier.Enqueue("Unexpected error. Please try again.");
					exceptionArgs.Handled = true;
				};

				MapperInitializer.Initialize(container);

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

				var shellView = container.GetInstance<ShellView>();

				application.Run(shellView);
			}
		}
	}
}
