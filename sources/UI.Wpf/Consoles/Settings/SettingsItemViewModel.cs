using Consoles.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Wpf.Consoles
{
	public class SettingsItemViewModel : BaseViewModel
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public SettingsItemViewModel()
		{

		}

		public Guid Id { get; set; }
		public int Index { get; set; }
		public string Name { get; set; }
		public string ProcessPathArgs { get; set; }
		public string ProcessPath { get; set; }
		public PathType ProcessPathType { get; set; }
	}
}
