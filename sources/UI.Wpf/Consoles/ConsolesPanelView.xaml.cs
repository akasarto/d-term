using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace UI.Wpf.Consoles
{
	public partial class ConsolesPanelView : UserControl, IViewFor<IConsolesPanelViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesPanelView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadOptionsCommand).SelectMany(x => x.Execute()).Subscribe());

				activator(ViewModel.OpenProcessInstanceViewInteraction.RegisterHandler(interaction =>
				{
					var viewModel = interaction.Input;

					var processInstanceView = new ConsoleInstanceView()
					{
						Owner = Application.Current.MainWindow,
						ViewModel = viewModel
					};

					processInstanceView.Events().SourceInitialized.Subscribe(args =>
					{
						var hwndSource = PresentationSource.FromVisual(processInstanceView) as HwndSource;
						var instanceViewWndProc = new ConsolenstanceWndProc(processInstanceView.ViewModel, hwndSource);
					});

					processInstanceView.Events().Closing.Subscribe(args =>
					{
						Application.Current.MainWindow.Activate();
					});

					processInstanceView.Show();

					interaction.SetOutput(new WindowInteropHelper(processInstanceView).Handle);
				}));

				activator(ViewModel.CloseProcessInstanceViewInteraction.RegisterHandler(interaction =>
				{
					var processInstanceView = HwndSource.FromHwnd(interaction.Input).RootVisual as Window;

					if (processInstanceView != null)
					{
						processInstanceView.Close();
						interaction.SetOutput(true);
						return;
					}

					interaction.SetOutput(false);
				}));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsolesPanelViewModel), typeof(ConsolesPanelView), new PropertyMetadata(null));

		public IConsolesPanelViewModel ViewModel
		{
			get { return (IConsolesPanelViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsolesPanelViewModel)value; }
		}
	}
}
