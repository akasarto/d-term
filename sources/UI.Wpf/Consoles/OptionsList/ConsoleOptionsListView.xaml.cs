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
	public partial class ConsoleOptionsListView : UserControl, IViewFor<ConsoleOptionsListViewModel>
	{
		public ConsoleOptionsListView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(x => x.ViewModel).Subscribe(viewModel =>
				{
					viewModel.Initialize();
				}));
			});
		}

		public ConsoleOptionsListViewModel ViewModel
		{
			get => (ConsoleOptionsListViewModel)DataContext;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ConsoleOptionsListViewModel)value; }
		}
	}
}
