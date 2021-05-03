using dTerm.Core;
using dTerm.Core.Reposistories;
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
    public class ShellProcessToolBarViewModel : BaseViewModel
    {
        private readonly ReadOnlyObservableCollection<ShellProcessToolbarOptionButtonViewModel> _optionButtons;
        private readonly ObservableCollectionExtended<ProcessEntity> _optionButtonsSource = new();
        private readonly IShellProcessesRepository _shellProcessesRepository;

        public ShellProcessToolBarViewModel(IShellProcessesRepository shellProcessesRepository = null)
        {
            _shellProcessesRepository = shellProcessesRepository ?? Locator.Current.GetService<IShellProcessesRepository>();

            _optionButtonsSource.ToObservableChangeSet().Transform(value =>
                new ShellProcessToolbarOptionButtonViewModel(_shellProcessesRepository, value)
            ).ObserveOn(RxApp.MainThreadScheduler).Bind(out _optionButtons).Subscribe();

            // Load shell option buttons
            LoadOptionButtons = ReactiveCommand.CreateFromTask(async () =>
            {
                var processes = await _shellProcessesRepository.ReadAllAsync();

                _optionButtonsSource.AddRange(processes);
            });
            LoadOptionButtons.IsExecuting.ToPropertyEx(this, x => x.OptionButtonsLoading);
            LoadOptionButtons.ThrownExceptions.Subscribe(ex => throw ex);
        }

        public ReadOnlyObservableCollection<ShellProcessToolbarOptionButtonViewModel> OptionButtons => _optionButtons;
        public ReactiveCommand<Unit, Unit> LoadOptionButtons { get; }
        [ObservableAsProperty] public bool OptionButtonsLoading { get; }
    }
}
