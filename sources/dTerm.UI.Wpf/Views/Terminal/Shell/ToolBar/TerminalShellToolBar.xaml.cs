using AutoMapper;
using dTerm.UI.Wpf.Mappings;
using dTerm.UI.Wpf.Stores;
using DynamicData;
using DynamicData.Binding;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace dTerm.UI.Wpf.Views
{
    public partial class TerminalShellToolBar : ToolBar
    {
        private readonly ShellsStore _shellsStore = new ShellsStore();
        private readonly IMapper _mapper = MapperFactory.Create();

        public TerminalShellToolBar()
        {
            InitializeComponent();
            ConfigureData();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await _shellsStore.LoadButtonOptions();

            loader.Visibility = Visibility.Collapsed;
            list.Visibility = Visibility.Visible;
        }

        private void ConfigureData()
        {
            ReadOnlyObservableCollection<TerminalShellToolbarOptionButtonViewModel> items;

            var sourceItems = _shellsStore.GetSharedItems();

            var sourceItemsObservable = sourceItems.Connect();

            _ = sourceItemsObservable
                .Transform(item => _mapper.Map<TerminalShellToolbarOptionButtonViewModel>(item))
                .Sort(SortExpressionComparer<TerminalShellToolbarOptionButtonViewModel>.Ascending(item => item.OrderIndex))
                .Bind(out items)
                .DisposeMany()
                .Subscribe()
            ;

            list.ItemsSource = items;
        }
    }
}
