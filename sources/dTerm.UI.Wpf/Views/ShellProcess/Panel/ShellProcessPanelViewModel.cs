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
    public class ShellProcessPanelViewModel : ReactiveObject
    {
        private readonly ObservableCollectionExtended<ProcessEntity> _buttonsSource;
        private readonly ReadOnlyObservableCollection<ShellProcessButtonViewModel> _processStartButtons;

        public ShellProcessPanelViewModel()
        {
            _buttonsSource = new ObservableCollectionExtended<ProcessEntity>();

            _buttonsSource.ToObservableChangeSet()
                .Transform(value => new ShellProcessButtonViewModel(value))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _processStartButtons)
                .Subscribe();

            Add = ReactiveCommand.CreateFromObservable(AddImpl);

            Load = ReactiveCommand.CreateFromObservable(LoadImpl);
            Load.IsExecuting.ToPropertyEx(this, x => x.IsLoading);
            Load.ThrownExceptions.Subscribe(ex => throw ex);
        }

        public ReadOnlyObservableCollection<ShellProcessButtonViewModel> ProcessStartButtons => _processStartButtons;

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

        [ObservableAsProperty] public bool IsLoading { get; }

        public ReactiveCommand<Unit, Unit> Add { get; }

        public ReactiveCommand<Unit, Unit> Load { get; }

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
