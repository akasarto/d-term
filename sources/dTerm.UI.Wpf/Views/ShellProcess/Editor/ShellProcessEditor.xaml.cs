using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessEditorBase : BaseUserControl<ShellProcessEditorViewModel>
    {
    }

    public partial class ShellProcessEditor : ShellProcessEditorBase
    {
        public ShellProcessEditor()
        {
            InitializeComponent();

            ViewModel = new ShellProcessEditorViewModel();

            this.WhenActivated(bindings =>
            {
                // Name textbox
                //this.Bind(
                //    ViewModel,
                //    vm => vm.Name,
                //    v => v.nameTextBox.Text
                //).DisposeWith(bindings);
                Binding myBinding = new Binding();
                myBinding.Source = ViewModel;
                myBinding.Path = new PropertyPath("Name");
                myBinding.Mode = BindingMode.TwoWay;
                myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                myBinding.NotifyOnValidationError = true;
                myBinding.ValidatesOnDataErrors = true;
                BindingOperations.SetBinding(nameTextBox, TextBox.TextProperty, myBinding);

                // Args
                this.Bind(
                    ViewModel,
                    vm => vm.Args,
                    v => v.argsTextBox.Text
                ).DisposeWith(bindings);

                // Cancel button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Cancel,
                    v => v.cancelButton
                ).DisposeWith(bindings);

                // Save button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Save,
                    v => v.saveButton
                ).DisposeWith(bindings);
            });
        }
    }
}
