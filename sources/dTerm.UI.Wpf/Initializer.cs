using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dTerm.UI.Wpf
{
	public class Initializer
	{
		public static Container CreateContainer()
		{
			var container = new Container();
			var assemblies = GetKnownAssemblies();

			//
			//container.Register<ShellViewModel>();

			//
			//container.Register<IConsoleService, ConsoleService>();
			//container.Register<IShellService, ShellService>();

			//
			container.Verify();

			return container;
		}

		private static IEnumerable<Assembly> GetKnownAssemblies()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			return assemblies.Where(assembly =>
				assembly.FullName.StartsWith("Sarto")
			).ToList();
		}
	}
}
