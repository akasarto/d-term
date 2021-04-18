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

            ViewModel = new IconBrowserViewModel();

            this.WhenActivated(bindings =>
            {
                DataContext = ViewModel;

                // Track selected icon
                this.Bind(
                    ViewModel,
                    vm => vm.SelectedIcon,
                    v => v.iconsList.SelectedItem
                ).DisposeWith(bindings);

                // Icons list
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Icons,
                    v => v.iconsList.ItemsSource
                ).DisposeWith(bindings);

                // Cancel button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Cancel,
                    v => v.cancelButton
                ).DisposeWith(bindings);

                // Save button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Save,
                    v => v.saveButton
                ).DisposeWith(bindings);

                // Search field
                this.Bind(
                    ViewModel,
                    vm => vm.SearchText,
                    v => v.searchBox.Text
                ).DisposeWith(bindings);

                // Trigger initial data load
                this.WhenAnyValue(x =>
                    x.ViewModel.Load
                ).SelectMany(x =>
                    x.Execute()
                ).ObserveOn(RxApp.MainThreadScheduler).Subscribe().DisposeWith(bindings);

                // Loader visibility
                this.WhenAnyValue(x =>
                    x.ViewModel.IsLoading
                ).Select(loading =>
                    loading ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed
                ).BindTo(
                    this,
                    view => view.loadingWrapper.Visibility
                ).DisposeWith(bindings);

                // Icons list visibility
                this.WhenAnyValue(x =>
                    x.ViewModel.IsLoading
                ).Select(loading =>
                    loading ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible
                ).BindTo(
                    this,
                    view => view.iconsListWrapper.Visibility
                ).DisposeWith(bindings);
            });
        }
    }
}
