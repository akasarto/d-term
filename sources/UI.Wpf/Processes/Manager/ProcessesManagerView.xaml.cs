using ReactiveUI;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System;
using MaterialDesignThemes.Wpf;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Processes manager view.
	/// </summary>
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
				activator(this.WhenAnyValue(@this => @this.ViewModel.LoadProcessesReactiveCommand).SelectMany(x => x.Execute()).Subscribe());
				activator(this.WhenAnyValue(@this => @this.ViewModel.IsLoadingProcesses).Subscribe(loading =>
				{
					listActions.IsHitTestVisible = !loading;
					listActions.Focusable = !loading;
					listActions.IsEnabled = !loading;
				}));
				activator(this.WhenAnyValue(@this => @this.ViewModel.ProcessViewModel).Subscribe(data =>
				{
					deleteButton.Visibility = data?.Id != Guid.Empty ? Visibility.Visible : Visibility.Collapsed;

					contextIcon.Kind = data == null ? PackIconKind.FormatListBulleted : data.Id == Guid.Empty ? PackIconKind.Plus : PackIconKind.Pencil;
					contextLabel.Text = data == null ? "Processes List" : data.Id == Guid.Empty ? "Add Process" : "Edit Process";

					listActions.Visibility = data == null ? Visibility.Visible : Visibility.Collapsed;
					formActions.Visibility = data == null ? Visibility.Collapsed : Visibility.Visible;
				}));
			});
		}

		/// <summary>
		/// View model dependency property backing field.
		/// </summary>
		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IProcessesManagerViewModel), typeof(ProcessesManagerView), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		public IProcessesManagerViewModel ViewModel
		{
			get { return (IProcessesManagerViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view model instance.
		/// </summary>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IProcessesManagerViewModel)value; }
		}
	}
}
