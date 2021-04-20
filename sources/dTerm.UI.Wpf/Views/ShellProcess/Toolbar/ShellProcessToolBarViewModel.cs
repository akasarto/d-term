using dTerm.UI.Wpf.Services;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessToolBarViewModel : BaseReactiveObject
    {
        private readonly ReadOnlyObservableCollection<ShellProcessToolbarButtonViewModel> _buttons;
        private readonly ShellProcessesData _shellProcessesData = new();

        public ShellProcessToolBarViewModel()
        {
            _shellProcessesData.ToObservableChangeSet()
                .Transform(value => new ShellProcessToolbarButtonViewModel(value))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _buttons)
                .Subscribe();

            //
            Add = ReactiveCommand.CreateFromTask(AddImpl);

            //
            Load = ReactiveCommand.CreateFromTask(LoadImpl);
            Load.IsExecuting.ToPropertyEx(this, x => x.IsLoading);
            Load.ThrownExceptions.Subscribe(ex => throw ex);
        }

        public ReadOnlyObservableCollection<ShellProcessToolbarButtonViewModel> Buttons => _buttons;

        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Load { get; }

        [ObservableAsProperty] public bool IsLoading { get; }

        private async Task<Unit> AddImpl()
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();

            fileDialog.DefaultExt = ".exe";
            fileDialog.Filter = "Executable Files (*.exe)|*.exe";
            fileDialog.Title = "Locate New Shell Process File";

            var result = fileDialog.ShowDialog();

            if (result ?? false)
            {
                await _shellProcessesData.Add(fileDialog.FileName);
            }

            return Unit.Default;
        }

        private async Task<Unit> LoadImpl()
        {
            await _shellProcessesData.LoadASync();

            return Unit.Default;
        }
    }
}
