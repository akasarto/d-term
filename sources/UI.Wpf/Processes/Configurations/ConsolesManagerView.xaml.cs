using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	public partial class ConsolesManagerView : UserControl, IViewFor<IConsolesManagerViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesManagerView()
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
					contextLabel.Text = data == null ? "Options List" : data.Id == Guid.Empty ? "Add Console" : "Edit Console";

					listActions.Visibility = data == null ? Visibility.Visible : Visibility.Collapsed;
					formActions.Visibility = data == null ? Visibility.Collapsed : Visibility.Visible;
				}));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsolesManagerViewModel), typeof(ConsolesManagerView), new PropertyMetadata(null));

		public IConsolesManagerViewModel ViewModel
		{
			get { return (IConsolesManagerViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsolesManagerViewModel)value; }
		}
	}
}
