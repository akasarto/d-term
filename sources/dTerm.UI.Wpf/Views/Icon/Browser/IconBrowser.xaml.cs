using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace dTerm.UI.Wpf.Views
{
    public abstract class IconBrowserBase : BaseUserControl<IconBrowserViewModel> { }

    public partial class IconBrowser : IconBrowserBase
    {
        public IconBrowser()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                ViewModel ??= new IconBrowserViewModel();

                // Track selected icon
                this.Bind(
                    ViewModel,
                    vm => vm.SelectedIcon,
                    v => v.iconsList.SelectedItem
                ).DisposeWith(disposables);

                // Icons list
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Icons,
                    v => v.iconsList.ItemsSource
                ).DisposeWith(disposables);

                // Cancel button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Cancel,
                    v => v.cancelButton
                ).DisposeWith(disposables);

                // Save button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Save,
                    v => v.saveButton
                ).DisposeWith(disposables);

                // Search field
                this.Bind(
                    ViewModel,
                    vm => vm.SearchText,
                    v => v.searchBox.Text
                ).DisposeWith(disposables);

                // Trigger initial data load
                this.WhenAnyValue(x =>
                    x.ViewModel.Load
                ).SelectMany(x =>
                    x.Execute()
                ).ObserveOn(RxApp.MainThreadScheduler).Subscribe().DisposeWith(disposables);

                // Loader visibility
                this.WhenAnyValue(x =>
                    x.ViewModel.IsLoading
                ).Select(loading =>
                    loading ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed
                ).BindTo(
                    this,
                    view => view.loadingWrapper.Visibility
                ).DisposeWith(disposables);

                // Icons list visibility
                this.WhenAnyValue(x =>
                    x.ViewModel.IsLoading
                ).Select(loading =>
                    loading ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible
                ).BindTo(
                    this,
                    view => view.iconsListWrapper.Visibility
                ).DisposeWith(disposables);
            });
        }
    }
}
