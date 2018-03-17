using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	public partial class ProcessesManagerView : UserControl, IViewFor<IProcessesManagerViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesManagerView()
		{
			InitializeComponent();

			this.WhenActivated(activator =>
			{
				activator(this.WhenAnyValue(@this => @this.ViewModel).BindTo(this, @this => @this.DataContext));
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadProcessesCommand).SelectMany(x => x.Execute()).Subscribe());
				activator(this.WhenAnyValue(@this => @this.ViewModel.IsLoadingProcesses).Subscribe(loading =>
				{
					listActions.IsHitTestVisible = !loading;
					listActions.Focusable = !loading;
					listActions.IsEnabled = !loading;
				}));
				activator(this.WhenAnyValue(@this => @this.ViewModel.FormData).Subscribe(data =>
				{
					deleteButton.Visibility = data?.Id != Guid.Empty ? Visibility.Visible : Visibility.Collapsed;

					contextIcon.Kind = data == null ? PackIconKind.FormatListBulleted : data.Id == Guid.Empty ? PackIconKind.Plus : PackIconKind.Pencil;
					contextLabel.Text = data == null ? Properties.Resources.OptionsList : data.Id == Guid.Empty ? Properties.Resources.AddProcess : Properties.Resources.EditProcess;

					listActions.Visibility = data == null ? Visibility.Visible : Visibility.Collapsed;
					formActions.Visibility = data == null ? Visibility.Collapsed : Visibility.Visible;
				}));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessesManagerViewModel), typeof(ProcessesManagerView), new PropertyMetadata(null));

		public IProcessesManagerViewModel ViewModel
		{
			get { return (IProcessesManagerViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessesManagerViewModel)value; }
		}
	}
}
