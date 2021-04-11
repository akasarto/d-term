using dTerm.Core;
using dTerm.Infra.EfCore;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace dTerm.UI.Wpf.UserControls
{
    public class ShellProcessesPanelViewModel : ReactiveObject
    {
        private readonly ReadOnlyObservableCollection<ShellProcessStartButtonViewModel> _processStartButtons;
        public ReadOnlyObservableCollection<ShellProcessStartButtonViewModel> ProcessStartButtons => _processStartButtons;

        public ObservableCollectionExtended<ProcessEntity> Source { get; }

        public ShellProcessesPanelViewModel()
        {
            Source = new ObservableCollectionExtended<ProcessEntity>();

            Source.ToObservableChangeSet()
                .Transform(value => new ShellProcessStartButtonViewModel(value))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _processStartButtons)
                .Subscribe();

            Load = ReactiveCommand.CreateFromObservable(LoadImpl);
            Load.IsExecuting.ToPropertyEx(this, x => x.IsLoading);
            Load.ThrownExceptions.Subscribe(ex => throw ex);
        }

        [ObservableAsProperty] public bool IsLoading { get; }

        public ReactiveCommand<Unit, Unit> Load { get; }

        private IObservable<Unit> LoadImpl() => Observable.Start(() =>
        {
            using (var context = new AppDbContext())
            {
                var entities = context.Consoles.ToList();

                foreach (var entity in entities)
                {
                    Source.Add(entity);
                }
            }
        });
    }
}
