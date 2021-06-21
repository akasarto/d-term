using dTerm.UI.Wpf.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessToolBarViewModel : BaseViewModel
    {
        private readonly ShellProcessesService _shellProcessData;

        public ShellProcessToolBarViewModel(ShellProcessesService shellProcessData = null)
        {
            _shellProcessData = shellProcessData ?? Locator.Current.GetService<ShellProcessesService>();

            // Load shell option buttons
            LoadOptionButtons = ReactiveCommand.CreateFromTask(async () => await _shellProcessData.LoadOptionButtonsAsync());
            LoadOptionButtons.IsExecuting.ToPropertyEx(this, x => x.OptionButtonsLoading);
            LoadOptionButtons.ThrownExceptions.Subscribe(ex => throw ex);
        }

        public ReactiveCommand<Unit, Unit> LoadOptionButtons { get; }
        [ObservableAsProperty] public bool OptionButtonsLoading { get; }
        public ReadOnlyObservableCollection<ShellProcessToolbarOptionButtonViewModel> OptionButtons => _shellProcessData.GetOptionButtons();
    }
}
