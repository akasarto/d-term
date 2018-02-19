using UI.Wpf.Shell;
using SimpleInjector;
using System;
using UI.Wpf.Mappings;
using System.Linq;
using MaterialDesignThemes.Wpf;
using System.Reflection;
using System.Globalization;
using System.IO;

namespace UI.Wpf
{
	public class StartupEntryPoint
	{
		[STAThread]
		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

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

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			AssemblyName assemblyName = new AssemblyName(args.Name);

			var path = assemblyName.Name + ".dll";

			Console.WriteLine($"Resolving: {path}");

			if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false) path = String.Format(@"{0}\{1}", assemblyName.CultureInfo, path);

			using (Stream stream = executingAssembly.GetManifestResourceStream(path))
			{
				if (stream == null) return null;
				var assemblyRawBytes = new byte[stream.Length];
				stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
				return Assembly.Load(assemblyRawBytes);
			}
		}
	}
}
