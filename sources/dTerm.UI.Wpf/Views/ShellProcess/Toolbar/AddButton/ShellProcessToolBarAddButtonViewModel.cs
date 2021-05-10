using dTerm.UI.Wpf.Services;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessToolBarAddButtonViewModel : BaseViewModel
    {
        private readonly Interaction<Unit, string> _fileSelector;
        private readonly ShellProcessData _shellProcessData;

        public ShellProcessToolBarAddButtonViewModel(ShellProcessData shellProcessData = null)
        {
            _fileSelector = new Interaction<Unit, string>();
            _shellProcessData = shellProcessData ?? Locator.Current.GetService<ShellProcessData>();

            Add = ReactiveCommand.CreateFromTask(async () =>
            {
                _ = _fileSelector.Handle(Unit.Default).Subscribe(async shell =>
                {
                    if (!string.IsNullOrWhiteSpace(shell))
                    {
                        await _shellProcessData.CreateNewAsync(shell);
                    }
                });

                return await Task.FromResult(Unit.Default);
            });
        }

        public Interaction<Unit, string> FileSelector => _fileSelector;
        public ReactiveCommand<Unit, Unit> Add { get; }
    }
}
