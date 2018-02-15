using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Wpf.Consoles
{
	public partial class ConsoleOptionsListView : UserControl, ISupportsActivation
	{
		public ConsoleOptionsListView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(x => x.DataContext).Subscribe(ctx =>
				{
					var viewModel = ctx as ConsoleOptionsListViewModel;

					viewModel?.Initialize();
				}));
			});
		}

		public ViewModelActivator Activator => new ViewModelActivator();
	}
}
