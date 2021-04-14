using DynamicData;
using DynamicData.Binding;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessEditorViewModel : ReactiveObject
    {
        private readonly ReadOnlyObservableCollection<IconEditorViewModel> _icons;

        public ShellProcessEditorViewModel()
        {
            var primaryIcons = Enum.GetNames<PackIconKind>().GroupBy(k =>
                Enum.Parse<PackIconKind>(k)
            ).Select(group => (Kind: group.Key, Aliases: group.ToList()));

            var iconsSource = new ObservableCollectionExtended<(PackIconKind, List<string>)>(primaryIcons.Take(3));

            iconsSource.ToObservableChangeSet()
                .Transform(item =>
                {
                    var (kind, aliases) = item;

                    return new IconEditorViewModel(kind, aliases);
                })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _icons)
                .Subscribe();
        }

        public ReadOnlyObservableCollection<IconEditorViewModel> Icons => _icons;
    }
}
