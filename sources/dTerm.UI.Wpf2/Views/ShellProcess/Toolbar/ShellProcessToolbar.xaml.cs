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

            this.WhenActivated(disposables =>
            {
                ViewModel ??= new ShellProcessToolBarViewModel();

                // Start buttons list
                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.OptionButtons,
                    view => view.optionButtonsList.ItemsSource
                ).DisposeWith(disposables);

                // Loader visibility
                this.WhenAnyValue(x =>
                    x.ViewModel.OptionButtonsLoading
                ).Select(loading =>
                    loading ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed
                ).BindTo(
                    this,
                    view => view.loader.Visibility
                ).DisposeWith(disposables);

                // Actions panel visibility
                this.WhenAnyValue(x =>
                    x.ViewModel.OptionButtonsLoading
                ).Select(loading =>
                    loading ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible
                ).BindTo(
                    this,
                    view => view.optionButtonsList.Visibility
                ).DisposeWith(disposables);

                // Trigger initial data load
                this.WhenAnyValue(x =>
                    x.ViewModel.LoadOptionButtons
                ).SelectMany(x =>
                    x.Execute()
                ).ObserveOn(RxApp.MainThreadScheduler).Subscribe().DisposeWith(disposables);
            });
        }
    }
}
