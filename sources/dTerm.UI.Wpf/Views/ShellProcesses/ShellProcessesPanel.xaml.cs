using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media;

namespace dTerm.UI.Wpf.UserControls
{
    public abstract class ShellProcessesPanelBase : BaseUserControl<ShellProcessesPanelViewModel> { }

    public partial class ShellProcessesPanel : ShellProcessesPanelBase
    {
        public ShellProcessesPanel()
        {
            InitializeComponent();

            ViewModel = new ShellProcessesPanelViewModel();

            this.WhenActivated(binding =>
            {
                // Bind Items
                this.OneWayBind(
                    ViewModel!,
                    viewModel => viewModel.ProcessStartButtons,
                    view => view.processStartButtonsList.ItemsSource
                ).DisposeWith(binding);

                // Loader visibility
                this.WhenAnyValue(x => x.ViewModel.IsLoading)
                    .Select(loading => loading ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed)
                    .BindTo(this, view => view.loader.Visibility)
                    .DisposeWith(binding);

                // Actions panel visibility
                this.WhenAnyValue(x => x.ViewModel.IsLoading)
                    .Select(loading => loading ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible)
                    .BindTo(this, view => view.processStartButtonsList.Visibility)
                    .DisposeWith(binding);

                // Load data
                this.WhenAnyValue(x => x.ViewModel.Load)
                    .SelectMany(x => x.Execute())
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe();
            });
        }
    }
}
