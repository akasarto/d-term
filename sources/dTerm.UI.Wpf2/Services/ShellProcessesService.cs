using AutoMapper;
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
    public class ShellProcessesService
    {
        private readonly IMapper _mapper;
        private readonly IShellProcessesRepository _shellProcessesRepository;
        private readonly ObservableCollectionExtended<ShellProcess> _optionButtonsSource = new();
        private readonly ReadOnlyObservableCollection<ShellProcessToolbarOptionButtonViewModel> _optionButtonsList;

        public ShellProcessesService(IMapper mapper = null, IShellProcessesRepository shellProcessesRepository = null)
        {
            _mapper = mapper ?? Locator.Current.GetService<IMapper>();
            _shellProcessesRepository = shellProcessesRepository ?? Locator.Current.GetService<IShellProcessesRepository>();

            _optionButtonsSource
                .ToObservableChangeSet()
                .Transform(value => _mapper.Map<ShellProcess, ShellProcessToolbarOptionButtonViewModel>(value))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _optionButtonsList)
                .Subscribe()
            ;
        }

        public async Task LoadOptionButtonsAsync()
        {
            var processes = await _shellProcessesRepository.ReadAllAsync();

            _optionButtonsSource.AddRange(processes);
        }

        public async Task CreateNewAsync(string shellExePath)
        {
            var shellProcess = new ShellProcess()
            {
                Id = Guid.NewGuid(),
                Icon = PackIconKind.CubeOutline.ToString(),
                Name = Path.GetFileNameWithoutExtension(shellExePath),
                ProcessExecutablePath = shellExePath,
                ProcessStartupArgs = string.Empty,
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
            shellProcess.ProcessStartupArgs = shellProcessEditorViewModel.ProcessStartupArgs;

            _ = await _shellProcessesRepository.UpdateAsync(shellProcess);
        }

        public async Task DeleteAsync(Guid shellProcessId)
        {
            await _shellProcessesRepository.DeleteAsync(shellProcessId);

            var sourceItem = _optionButtonsSource.SingleOrDefault(s => s.Id.Equals(shellProcessId));

            if (sourceItem != null)
            {
                _optionButtonsSource.Remove(sourceItem);
            }
        }

        public async Task UpdateIconAsync(Guid shellProcessId, IconBrowserViewModel iconBrowserViewModel)
        {
            var shellProcess = await _shellProcessesRepository.ReadByIdAsync(shellProcessId);

            shellProcess.Icon = iconBrowserViewModel.SelectedIcon.Kind.ToString();

            _ = await _shellProcessesRepository.UpdateAsync(shellProcess);
        }

        public async Task<ShellProcess> GetByIdAsync(Guid shellProcessId) => await _shellProcessesRepository.ReadByIdAsync(shellProcessId);

        public ReadOnlyObservableCollection<ShellProcessToolbarOptionButtonViewModel> GetOptionButtons() => _optionButtonsList;
    }
}
