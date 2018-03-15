using System.Security.Principal;

namespace UI.Wpf
{
	public interface IAppContext
	{
		bool IsAdmin { get; }
	}

	public class AppContext : IAppContext
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public AppContext()
		{
		}

		public bool IsAdmin
		{
			get
			{
				using (var identity = WindowsIdentity.GetCurrent())
				{
					var principal = new WindowsPrincipal(identity);
					return principal.IsInRole(WindowsBuiltInRole.Administrator);
				}
			}
		}
	}
}
