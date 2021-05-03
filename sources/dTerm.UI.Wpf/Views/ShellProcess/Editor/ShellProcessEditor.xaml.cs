﻿using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessEditorBase : BaseUserControl<ShellProcessEditorViewModel> { }

    public partial class ShellProcessEditor : ShellProcessEditorBase
    {
        public ShellProcessEditor()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                // Icon
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Icon,
                    v => v.shellIcon.Kind
                ).DisposeWith(disposables);

                // Executable Path
                this.OneWayBind(
                    ViewModel,
                    vm => vm.ExePath,
                    v => v.exePath.Text
                ).DisposeWith(disposables);

                // Name textbox
                // Needs native binding for validation.
                _ = BindingOperations.SetBinding(nameTextBox, TextBox.TextProperty, new Binding()
                {
                    Mode = BindingMode.TwoWay,
                    Path = new PropertyPath(nameof(ViewModel.Name)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    NotifyOnValidationError = true,
                    ValidatesOnDataErrors = true,
                    Source = ViewModel
                });

                // Executable args
                // Needs native binding for validation.
                _ = BindingOperations.SetBinding(argsTextBox, TextBox.TextProperty, new Binding()
                {
                    Mode = BindingMode.TwoWay,
                    Path = new PropertyPath(nameof(ViewModel.ExeArgs)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    NotifyOnValidationError = true,
                    ValidatesOnDataErrors = true,
                    Source = ViewModel
                });

                // Cancel button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Cancel,
                    v => v.cancelButton
                ).DisposeWith(disposables);

                // Save button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Save,
                    v => v.saveButton
                ).DisposeWith(disposables);
            });
        }
    }
}