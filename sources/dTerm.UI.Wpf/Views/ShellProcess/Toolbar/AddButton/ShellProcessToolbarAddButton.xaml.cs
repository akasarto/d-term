using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessToolBarAddButtonBase : BaseUserControl<ShellProcessToolBarAddButtonViewModel> { }

    public partial class ShellProcessToolBarAddButton : ShellProcessToolBarAddButtonBase
    {
        public ShellProcessToolBarAddButton()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                ViewModel ??= new ShellProcessToolBarAddButtonViewModel();

                //
                disposables.Add(ViewModel.FileSelector.RegisterHandler(interaction =>
                {
                    var fileDialog = new Microsoft.Win32.OpenFileDialog()
                    {
                        DefaultExt = ".exe",
                        Filter = Core.Localization.Content.Executable_Files_Exe,
                        Title = Core.Localization.Content.Locate_New_Shell_Process_File
                    };

                    var fileName = (fileDialog.ShowDialog() ?? false) ? fileDialog.FileName : string.Empty;

                    interaction.SetOutput(fileName);
                }).DisposeWith(disposables));

                //
                this.BindCommand(
                    ViewModel,
                    vm => vm.Add,
                    v => v.add
                ).DisposeWith(disposables);
            });
        }
    }
}
