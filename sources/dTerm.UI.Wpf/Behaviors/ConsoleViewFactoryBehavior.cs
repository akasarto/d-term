using dTerm.Core;
using dTerm.UI.Wpf.ViewModels;
using dTerm.UI.Wpf.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Interactivity;

namespace dTerm.UI.Wpf.Behaviors
{
	public class ConsoleViewFactoryBehavior : Behavior<Window>
	{
		public static readonly DependencyProperty ConsoleInstancesProperty = DependencyProperty.Register("ConsoleInstances", typeof(ObservableCollection<IConsoleInstance>), typeof(ConsoleViewFactoryBehavior), new PropertyMetadata(null));

		public ObservableCollection<IConsoleInstance> ConsoleInstances
		{
			get { return (ObservableCollection<IConsoleInstance>)GetValue(ConsoleInstancesProperty); }
			set { SetValue(ConsoleInstancesProperty, value); }
		}

		protected override void OnAttached()
		{
			AssociatedObject.Loaded += AssociatedObject_Loaded;
		}

		private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
		{
			ConsoleInstances.CollectionChanged += ConsoleInstances_CollectionChanged;
		}

		private void ConsoleInstances_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems.Count > 0)
			{
				foreach (IConsoleInstance instance in e.NewItems)
				{
					var consoleView = new ConsoleView();

					consoleView.ShowInTaskbar = false;
					consoleView.ShowActivated = false;
					consoleView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
					consoleView.DataContext = new ConsoleViewModel(instance);
					consoleView.ResizeMode = ResizeMode.CanResize;
					consoleView.Owner = AssociatedObject;

					consoleView.Show();
				}
			}
		}
	}
}
