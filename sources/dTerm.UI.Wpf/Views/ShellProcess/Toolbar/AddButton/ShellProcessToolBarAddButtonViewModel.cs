using dTerm.Core;
using dTerm.Core.Reposistories;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using Splat;
using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessToolBarAddButtonViewModel : BaseViewModel
    {
        private readonly Interaction<Unit, string> _fileSelector;
        private readonly IShellProcessesRepository _shellProcessesRepository;

        public ShellProcessToolBarAddButtonViewModel(IShellProcessesRepository shellProcessesRepository = null)
        {
            _fileSelector = new Interaction<Unit, string>();
            _shellProcessesRepository = shellProcessesRepository ?? Locator.Current.GetService<IShellProcessesRepository>();

            Add = ReactiveCommand.CreateFromTask(AddImpl);
        }

        public ReactiveCommand<Unit, Unit> Add { get; }
        public Interaction<Unit, string> FileSelector => _fileSelector;

        private async Task<Unit> AddImpl()
        {
            _ = _fileSelector.Handle(Unit.Default).Subscribe(async shell =>
            {
                if (!string.IsNullOrWhiteSpace(shell))
                {
                    var shellProcess = new ProcessEntity()
                    {
                        Id = Guid.NewGuid(),
                        Icon = PackIconKind.CubeOutline.ToString(),
                        Name = Path.GetFileNameWithoutExtension(shell),
                        ProcessExecutablePath = shell,
                        ProcessStartupArgs = string.Empty,
                        ProcessType = ProcessType.Shell,
                        UTCCreation = DateTime.UtcNow,
                        OrderIndex = int.MaxValue,
                    };

                    _ = await _shellProcessesRepository.CreateAsync(shellProcess);
                }
            });

            return await Task.FromResult(Unit.Default);
        }
    }
}
