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
    public class IconBrowserViewModel : BaseViewModel
    {
        private readonly ReadOnlyObservableCollection<IconBrowserItemViewModel> _icons;
        private readonly ObservableCollectionExtended<(PackIconKind, List<string>)> _iconsSource = new();

        public IconBrowserViewModel()
        {
            // Check Selected
            var canSave = this
                .WhenAnyValue(vm => vm.SelectedIcon)
                .Select(icon => icon != null)
            ;

            // Load Icons List
            var iconFilter = this
                .WhenAnyValue(@this => @this.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(350), RxApp.TaskpoolScheduler)
                .DistinctUntilChanged()
                .Select(IconFilter)
            ;

            _iconsSource
                .ToObservableChangeSet()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Transform(item =>
                {
                    var (kind, aliases) = item;
                    return new IconBrowserItemViewModel(kind, aliases);
                })
                .Filter(iconFilter)
                .Bind(out _icons)
                .DisposeMany()
                .Subscribe()
            ;

            // Cancel Button
            Cancel = ReactiveCommand.Create(() => DialogHost.Close(DialogNames.Main, false));

            // Save Button
            Save = ReactiveCommand.Create(() => DialogHost.Close(DialogNames.Main, true), canSave);

            // Load Contents
            Load = ReactiveCommand.CreateFromObservable(() => Observable.Start(() =>
            {
                var primaryIcons = Enum
                    .GetNames<PackIconKind>()
                    .GroupBy(k => Enum.Parse<PackIconKind>(k))
                    .Select(group => (Kind: group.Key, Aliases: group.ToList()))
                ;
                _iconsSource.AddRange(primaryIcons.Take(5).ToList());
            }));
            Load.IsExecuting.ToPropertyEx(this, x => x.IsLoading);
            Load.ThrownExceptions.Subscribe(ex => throw ex);
        }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> Save { get; }
        public ReactiveCommand<Unit, Unit> Load { get; }

        public ReadOnlyObservableCollection<IconBrowserItemViewModel> Icons => _icons;

        [ObservableAsProperty] public bool IsLoading { get; }
        [Reactive] public IconBrowserItemViewModel SelectedIcon { get; set; }
        [Reactive] public string SearchText { get; set; }

        private Func<IconBrowserItemViewModel, bool> IconFilter(string query) => item =>
        {
            var kinds = string.Concat(
                item.Kind.ToString(),
                ".",
                string.Join(".", item.Aliases)
            );

            return kinds.Contains(query, StringComparison.OrdinalIgnoreCase);
        };
    }
}
