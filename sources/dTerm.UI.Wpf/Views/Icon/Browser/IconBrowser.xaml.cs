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
                // Icons
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Icons,
                    v => v.icons.ItemsSource
                ).DisposeWith(bindings);

                // Cancel
                this.BindCommand(
                    ViewModel,
                    vm => vm.Cancel,
                    v => v.cancel
                ).DisposeWith(bindings);

                // Save
                this.BindCommand(
                    ViewModel,
                    vm => vm.Save,
                    v => v.save
                ).DisposeWith(bindings);

                // Load data
                this.WhenAnyValue(x => x.ViewModel.Load)
                    .SelectMany(x => x.Execute())
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe();

                // Track Selected Item
                this.Bind(
                    ViewModel,
                    vm => vm.SelectedIcon,
                    v => v.icons.SelectedItem
                );
            });
        }
    }
}
