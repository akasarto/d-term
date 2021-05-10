using dTerm.Core;
using dTerm.Core.Reposistories;
using dTerm.UI.Wpf.Views;
using DynamicData;
using DynamicData.Binding;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.Services
{
    public class ShellProcessData
    {
        private readonly IShellProcessesRepository _shellProcessesRepository;
        private readonly ObservableCollectionExtended<ProcessEntity> _optionButtonsSource = new();
        private readonly ReadOnlyObservableCollection<ShellProcessToolbarOptionButtonViewModel> _optionButtonsList;

        public ShellProcessData(IShellProcessesRepository shellProcessesRepository = null)
        {
            _shellProcessesRepository = shellProcessesRepository ?? Locator.Current.GetService<IShellProcessesRepository>();

            _optionButtonsSource.ToObservableChangeSet().Transform(value =>
                new ShellProcessToolbarOptionButtonViewModel(this, value)
            ).ObserveOn(RxApp.MainThreadScheduler).Bind(out _optionButtonsList).Subscribe();
        }

        public async Task LoadOptionButtonsAsync()
        {
            var processes = await _shellProcessesRepository.ReadAllAsync();

            _optionButtonsSource.AddRange(processes);
        }

        public async Task CreateNewAsync(string shellExePath)
        {
            var shellProcess = new ProcessEntity()
            {
                Id = Guid.NewGuid(),
                Icon = PackIconKind.CubeOutline.ToString(),
                Name = Path.GetFileNameWithoutExtension(shellExePath),
                ProcessExecutablePath = shellExePath,
                ProcessStartupArgs = string.Empty,
                ProcessType = ProcessType.Shell,
                UTCCreation = DateTime.UtcNow,
                OrderIndex = int.MaxValue,
            };

            shellProcess = await _shellProcessesRepository.CreateAsync(shellProcess);

            _optionButtonsSource.Add(shellProcess);
        }

        public async Task UpdateBasicInfoAsync(ShellProcessEditorViewModel shellProcessEditorViewModel)
        {
            var shellProcess = await _shellProcessesRepository.ReadByIdAsync(shellProcessEditorViewModel.Id);

            shellProcess.Name = shellProcessEditorViewModel.Name;
            shellProcess.ProcessStartupArgs = shellProcessEditorViewModel.ExeArgs;

            _ = await _shellProcessesRepository.UpdateAsync(shellProcess);
        }

        public async Task DeleteAsync(Guid shellProcessId)
        {
            await _shellProcessesRepository.DeleteAsync(shellProcessId);

            var soureItem = _optionButtonsSource.SingleOrDefault(s => s.Id.Equals(shellProcessId));

            if (soureItem != null)
            {
                _optionButtonsSource.Remove(soureItem);
            }
        }

        public async Task UpdateIconAsync(Guid shellProcessId, IconBrowserViewModel iconBrowserViewModel)
        {
            var shellProcess = await _shellProcessesRepository.ReadByIdAsync(shellProcessId);

            shellProcess.Icon = iconBrowserViewModel.SelectedIcon.Kind.ToString();

            _ = await _shellProcessesRepository.UpdateAsync(shellProcess);
        }

        public async Task<ShellProcessEditorViewModel> GetByIdAsync(Guid shellProcessId)
        {
            var shellProcess = await _shellProcessesRepository.ReadByIdAsync(shellProcessId);

            return new ShellProcessEditorViewModel(shellProcess);
        }

        public ReadOnlyObservableCollection<ShellProcessToolbarOptionButtonViewModel> GetOptionButtons() => _optionButtonsList;
    }
}
