using dTerm.Core;
using dTerm.Core.Reposistories;
using dTerm.Infra.EfCore.Repositories;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessToolBarViewModel : BaseViewModel, IActivatableViewModel
    {
        private readonly ReadOnlyObservableCollection<ShellProcessToolbarOptionButtonViewModel> _optionButtons;
        private readonly ObservableCollectionExtended<ProcessEntity> _optionButtonsSource = new();
        private readonly IShellProcessesRepository _shellProcessesRepository;

        public ShellProcessToolBarViewModel(IShellProcessesRepository shellProcessesRepository = null)
        {
            _shellProcessesRepository = shellProcessesRepository ?? Locator.Current.GetService<IShellProcessesRepository>();

            _optionButtonsSource.ToObservableChangeSet()
                .Transform(value => new ShellProcessToolbarOptionButtonViewModel(_shellProcessesRepository, value))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _optionButtons)
                .Subscribe();

            //
            LoadOptionButtons = ReactiveCommand.CreateFromTask(LoadImpl);
            LoadOptionButtons.IsExecuting.ToPropertyEx(this, x => x.OptionButtonsLoading);
            LoadOptionButtons.ThrownExceptions.Subscribe(ex => throw ex);
        }

        public ReadOnlyObservableCollection<ShellProcessToolbarOptionButtonViewModel> OptionButtons => _optionButtons;

        public ReactiveCommand<Unit, Unit> LoadOptionButtons { get; }

        [ObservableAsProperty] public bool OptionButtonsLoading { get; }

        public ViewModelActivator Activator => new ViewModelActivator();

        private async Task<Unit> LoadImpl()
        {
            var processes = await _shellProcessesRepository.ReadAllAsync();

            _optionButtonsSource.AddRange(processes);

            return Unit.Default;
        }
    }
}
