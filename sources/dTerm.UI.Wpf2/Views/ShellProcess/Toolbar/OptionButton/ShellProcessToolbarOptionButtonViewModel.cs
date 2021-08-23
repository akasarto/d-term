using AutoMapper;
using dTerm.UI.Wpf.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessToolbarOptionButtonViewModel : BaseViewModel
    {
        private readonly IMapper _mapper;
        private readonly ShellProcessesService _shellProcessesService;

        public ShellProcessToolbarOptionButtonViewModel(IMapper mapper = null, ShellProcessesService shellProcessesService = null)
        {
            _mapper = mapper ?? Locator.Current.GetService<IMapper>();
            _shellProcessesService = shellProcessesService ?? Locator.Current.GetService<ShellProcessesService>();

            // Launch
            Launch = ReactiveCommand.Create<ShellProcessTerminalViewModel>(windowViewModel =>
            {
                var window = windowViewModel.GetWindow();

                window.Owner = Application.Current.MainWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.ShowInTaskbar = false;

                window.Show();
            });
            Launch.ThrownExceptions.Subscribe(ex => throw ex);

            // Edit
            ShowShellProcessEditorDialog = new Interaction<ShellProcessEditorViewModel, bool>();
            Edit = ReactiveCommand.CreateFromTask(async () =>
            {
                var shellProcess = await _shellProcessesService.GetByIdAsync(Id);
                var shellProcessEditorViewModel = _mapper.Map<ShellProcessEditorViewModel>(shellProcess);

                _ = ShowShellProcessEditorDialog.Handle(shellProcessEditorViewModel).Where(trueResult => trueResult).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ =>
                {
                    await _shellProcessesService.UpdateBasicInfoAsync(shellProcessEditorViewModel);

                    Name = shellProcessEditorViewModel.Name;
                });
            });

            // Delete
            ConfirmDeletionDialog = new Interaction<string, bool>();
            Delete = ReactiveCommand.Create(() =>
            {
                _ = ConfirmDeletionDialog.Handle(Name).Where(trueResult => trueResult).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ =>
                {
                    await _shellProcessesService.DeleteAsync(Id);
                });
            });

            // Change Icon
            ShowIconBrowserDialog = new Interaction<IconBrowserViewModel, bool>();
            ChangeIcon = ReactiveCommand.Create(() =>
            {
                var iconBrowserViewModel = new IconBrowserViewModel();

                _ = ShowIconBrowserDialog.Handle(iconBrowserViewModel).Where(trueResult => trueResult).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ =>
                {
                    await _shellProcessesService.UpdateIconAsync(Id, iconBrowserViewModel);

                    Icon = iconBrowserViewModel.SelectedIcon.Kind.ToString();
                });
            });
        }

        [Reactive] public Guid Id { get; set; }
        [Reactive] public string Icon { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public string ProcessExecutablePath { get; set; }
        [Reactive] public string ProcessStartupArgs { get; set; }

        public ReactiveCommand<Unit, Unit> Edit { get; }
        public ReactiveCommand<Unit, Unit> Delete { get; }
        public ReactiveCommand<Unit, Unit> ChangeIcon { get; }
        public ReactiveCommand<ShellProcessTerminalViewModel, Unit> Launch { get; }
        public Interaction<IconBrowserViewModel, bool> ShowIconBrowserDialog { get; }
        public Interaction<ShellProcessEditorViewModel, bool> ShowShellProcessEditorDialog { get; }
        public Interaction<string, bool> ConfirmDeletionDialog { get; }
    }
}
