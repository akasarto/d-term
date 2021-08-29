using DynamicData;
using DynamicData.Binding;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace dTerm.UI.Wpf.Views
{
    public partial class IconBrowser : UserControl
    {
        private readonly SourceList<(PackIconKind kind, List<string> alliases)> _source = new();

        public IconBrowser()
        {
            InitializeComponent();
            ConfigureData();
        }

        private void ConfigureData()
        {
            ReadOnlyObservableCollection<IconBrowserItemViewModel> items;

            var sourceItemsObservable = _source.Connect();

            _ = sourceItemsObservable
                .Transform(item => new IconBrowserItemViewModel
                {
                    Aliases = string.Join(", ", item.alliases),
                    Kind = item.kind
                })
                .Sort(SortExpressionComparer<IconBrowserItemViewModel>.Ascending(item => item.Kind))
                .Bind(out items)
                .DisposeMany()
                .Subscribe()
            ;

            var view = CollectionViewSource.GetDefaultView(items);

            searchBox.KeyUp += (_, _) => view.Refresh();

            view.Filter = IconsFilter;

            list.ItemsSource = items;
        }

        private bool IconsFilter(object item)
        {
            var query = searchBox?.Text.ToLower().Trim();

            if (!string.IsNullOrEmpty(query) && item is IconBrowserItemViewModel iconBrowserItem)
            {
                var kinds = string.Concat(
                    iconBrowserItem.Kind.ToString(),
                    ".",
                    string.Join(".", iconBrowserItem.Aliases)
                );

                return kinds.Contains(query, StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var primaryIcons = Enum
                .GetNames<PackIconKind>()
                .GroupBy(k => Enum.Parse<PackIconKind>(k))
                .Select(group => (Kind: group.Key, Aliases: group.ToList()))
            ;

            _source.AddRange(primaryIcons);
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogHost.Close(DialogNames.Main, parameter: false);
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            DialogHost.Close(DialogNames.Main, parameter: true);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveButton.IsEnabled = list.SelectedItem != null;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _source.Dispose();
        }
    }
}
