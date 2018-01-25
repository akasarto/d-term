using CommandLine;

namespace UI.Wpf.Infrastructure
{
	public class StartupOptions
	{
		[Option(longName: "verify", HelpText = "Verify the dependency injection container integrity without running the application.")]
		public bool Verify { get; set; }
	}
}
