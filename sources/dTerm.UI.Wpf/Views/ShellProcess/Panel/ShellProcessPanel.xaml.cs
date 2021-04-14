using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessPanelBase : BaseUserControl<ShellProcessPanelViewModel> { }

    public partial class ShellProcessPanel : ShellProcessPanelBase
    {
        public ShellProcessPanel()
        {
            InitializeComponent();

            ViewModel = new ShellProcessPanelViewModel();

            this.WhenActivated(binding =>
            {
                // Add
                this.BindCommand(
                    ViewModel,
                    vm => vm.Add,
                    v => v.add
                );

                // Start Buttons
                this.OneWayBind(
                    ViewModel!,
                    viewModel => viewModel.ProcessStartButtons,
                    view => view.processStartButtons.ItemsSource
                ).DisposeWith(binding);

                // Loader visibility
                this.WhenAnyValue(x => x.ViewModel.IsLoading)
                    .Select(loading => loading ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed)
                    .BindTo(this, view => view.loader.Visibility)
                    .DisposeWith(binding);

                // Actions panel visibility
                this.WhenAnyValue(x => x.ViewModel.IsLoading)
                    .Select(loading => loading ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible)
                    .BindTo(this, view => view.processStartButtons.Visibility)
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
