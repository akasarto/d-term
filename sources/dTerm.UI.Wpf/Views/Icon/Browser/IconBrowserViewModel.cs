using DynamicData;
using DynamicData.Binding;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace dTerm.UI.Wpf.Views
{
    public class IconBrowserViewModel : ReactiveObject
    {
        private readonly ObservableCollectionExtended<(PackIconKind, List<string>)> _iconsSource;

        private ReadOnlyObservableCollection<IconBrowserItemViewModel> _icons;

        public IconBrowserViewModel()
        {
            var primaryIcons = Enum.GetNames<PackIconKind>().GroupBy(k =>
                Enum.Parse<PackIconKind>(k)
            ).Select(group => (Kind: group.Key, Aliases: group.ToList()));

            _iconsSource = new ObservableCollectionExtended<(PackIconKind, List<string>)>(primaryIcons);

            _iconsSource.ToObservableChangeSet()
                .Transform(item =>
                {
                    var (kind, aliases) = item;

                    return new IconBrowserItemViewModel(kind, aliases);
                })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _icons)
                .Subscribe();

            var canSave = this.WhenAnyValue(vm => vm.SelectedIcon).Select(icon => icon != null);

            Save = ReactiveCommand.CreateFromObservable(SaveImpl, canSave);
            Cancel = ReactiveCommand.Create(CancelImpl);
            Search = ReactiveCommand.CreateFromObservable(SearchImpl);
            Load = ReactiveCommand.CreateFromObservable(LoadImpl);
        }

        public ReadOnlyObservableCollection<IconBrowserItemViewModel> Icons => _icons;

        [Reactive] public IconBrowserItemViewModel SelectedIcon { get; set; }

        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> Search { get; }
        public ReactiveCommand<Unit, Unit> Load { get; }

        private void CancelImpl() => DialogHost.Close("shellProcessesPanel");

        private IObservable<Unit> SearchImpl() => Observable.Start(() =>
        {

        });

        private IObservable<Unit> SaveImpl() => Observable.Start(() =>
        {

        });

        private IObservable<Unit> LoadImpl() => Observable.Start(() =>
        {
        });
    }
}
