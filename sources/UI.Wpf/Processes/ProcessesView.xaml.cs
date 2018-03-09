using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace UI.Wpf.Processes
{
	public partial class ProcessesView : UserControl, IViewFor<IProcessesViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadOptionsCommand).SelectMany(x => x.Execute()).Subscribe());

				activator(ViewModel.OpenProcessInstanceViewInteraction.RegisterHandler(interaction =>
				{
					var viewModel = interaction.Input;

					var processInstanceView = new ProcessInstanceView()
					{
						Owner = Application.Current.MainWindow,
						ViewModel = viewModel
					};

					processInstanceView.Events().SourceInitialized.Subscribe(args =>
					{
						var hwndSource = PresentationSource.FromVisual(processInstanceView) as HwndSource;
						var instanceViewWndProc = new ProcessInstanceViewWndProc(processInstanceView.ViewModel, hwndSource);
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

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessesViewModel), typeof(ProcessesView), new PropertyMetadata(null));

		public IProcessesViewModel ViewModel
		{
			get { return (IProcessesViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessesViewModel)value; }
		}
	}
}
