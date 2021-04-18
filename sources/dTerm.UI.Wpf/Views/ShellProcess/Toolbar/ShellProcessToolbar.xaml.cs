using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessToolBarBase : BaseToolBarControl<ShellProcessToolBarViewModel> { }

    public partial class ShellProcessToolBar : ShellProcessToolBarBase
    {
        public ShellProcessToolBar()
        {
            InitializeComponent();

            ViewModel = new ShellProcessToolBarViewModel();

            this.WhenActivated(bindings =>
            {
                // Add button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Add,
                    v => v.add
                ).DisposeWith(bindings);

                // Start buttons list
                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.Buttons,
                    view => view.buttonsList.ItemsSource
                ).DisposeWith(bindings);

                // Loader visibility
                this.WhenAnyValue(x =>
                    x.ViewModel.IsLoading
                ).Select(loading =>
                    loading ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed
                ).BindTo(
                    this,
                    view => view.loader.Visibility
                ).DisposeWith(bindings);

                // Actions panel visibility
                this.WhenAnyValue(x =>
                    x.ViewModel.IsLoading
                ).Select(loading =>
                    loading ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible
                ).BindTo(
                    this,
                    view => view.buttonsList.Visibility
                ).DisposeWith(bindings);

                // Trigger initial data load
                this.WhenAnyValue(x =>
                    x.ViewModel.Load
                ).SelectMany(x =>
                    x.Execute()
                ).ObserveOn(RxApp.MainThreadScheduler).Subscribe().DisposeWith(bindings);
            });
        }
    }
}
