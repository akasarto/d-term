using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Processes view.
	/// </summary>
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
					var processInstanceView = new ProcessInstanceView()
					{
						Owner = Application.Current.MainWindow,
						ViewModel = interaction.Input
					};

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

		/// <summary>
		/// View model dependency property backing field.
		/// </summary>
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessesViewModel), typeof(ProcessesView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IProcessesViewModel ViewModel
		{
			get { return (IProcessesViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessesViewModel)value; }
		}
	}
}
