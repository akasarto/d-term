using ReactiveUI;
using System.Windows;
using System.Windows.Controls;

namespace dTerm.UI.Wpf.UserControls
{
    public abstract class BaseUserControl<TViewModel> : UserControl, IViewFor<TViewModel> where TViewModel : class
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(TViewModel),
            typeof(BaseUserControl<TViewModel>),
            new PropertyMetadata(default(TViewModel))
        );

        public TViewModel BindingRoot => ViewModel;

        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel)value;
        }
    }
}
