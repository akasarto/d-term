using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UI.Wpf.Processes
{
	public partial class ConsolesTabView : UserControl, IViewFor<IConsolesTabViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsolesTabView()
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
					contextLabel.Text = data == null ? Properties.Resources.OptionsList : data.Id == Guid.Empty ? Properties.Resources.AddConsole : Properties.Resources.EditConsole;

					listActions.Visibility = data == null ? Visibility.Visible : Visibility.Collapsed;
					formActions.Visibility = data == null ? Visibility.Collapsed : Visibility.Visible;
				}));
			});
		}

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IConsolesTabViewModel), typeof(ConsolesTabView), new PropertyMetadata(null));

		public IConsolesTabViewModel ViewModel
		{
			get { return (IConsolesTabViewModel)GetValue(ViewModelProperty); }
			set { SetValue(ViewModelProperty, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (IConsolesTabViewModel)value; }
		}
	}
}
