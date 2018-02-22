using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionsPanelViewModel : ReactiveObject, IConsoleOptionsPanelViewModel
	{
		public ConsoleOptionsPanelViewModel()
		{
		}

		public string Test => "Aleluia!";
	}
}
