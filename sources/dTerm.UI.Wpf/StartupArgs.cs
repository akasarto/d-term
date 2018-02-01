using CommandLine;

namespace dTerm.UI.Wpf.Infrastructure
{
	public class StartupArgs
	{
		[Option(longName: "verify", HelpText = "Verify the dependency injection container integrity without running the application.")]
		public bool Verify { get; set; }
	}
}
