using dTerm.Core;
using dTerm.Infra.EfCore;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessToolBarViewModel : BaseReactiveObject
    {
        private readonly ReadOnlyObservableCollection<ShellProcessToolbarButtonViewModel> _buttons;
        private readonly ObservableCollectionExtended<ProcessEntity> _buttonsSource = new();

        public ShellProcessToolBarViewModel()
        {
            _buttonsSource.ToObservableChangeSet()
                .Transform(value => new ShellProcessToolbarButtonViewModel(value))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _buttons)
                .Subscribe();

            Add = ReactiveCommand.CreateFromObservable(AddImpl);

            Load = ReactiveCommand.CreateFromObservable(LoadImpl);
            Load.IsExecuting.ToPropertyEx(this, x => x.IsLoading);
            Load.ThrownExceptions.Subscribe(ex => throw ex);
        }

        public ReadOnlyObservableCollection<ShellProcessToolbarButtonViewModel> Buttons => _buttons;

        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Load { get; }

        [ObservableAsProperty] public bool IsLoading { get; }

        private IObservable<Unit> AddImpl() => Observable.Start(() =>
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();

            fileDialog.DefaultExt = ".exe";
            fileDialog.Filter = "Executable Files (*.exe)|*.exe";
            fileDialog.Title = "Locate New Shell Process File";

            var result = fileDialog.ShowDialog();

            if (result ?? false)
            {
                var filename = fileDialog.FileName;

                _buttonsSource.Add(new ProcessEntity()
                {
                    Id = Guid.NewGuid(),
                    Icon = MaterialDesignThemes.Wpf.PackIconKind.CubeOutline.ToString(),
                    Name = Path.GetFileNameWithoutExtension(filename),
                    ProcessExecutablePath = filename,
                    ProcessStartupArgs = string.Empty,
                    ProcessType = ProcessType.Shell,
                    UTCCreation = DateTime.UtcNow,
                    OrderIndex = int.MaxValue,
                });
            }
        });

        private IObservable<Unit> LoadImpl() => Observable.Start(() =>
        {
            using (var context = new AppDbContext())
            {
                var entities = context.Consoles.ToList();

                foreach (var entity in entities)
                {
                    _buttonsSource.Add(entity);
                }
            }
        });
    }
}
