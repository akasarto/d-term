using System.Windows;
using System.Windows.Controls;

namespace dTerm.UI.Wpf.Views
{
    public abstract class BindableUserControl<TViewModel> : UserControl where TViewModel : class
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(TViewModel),
            typeof(BindableUserControl<TViewModel>),
            new PropertyMetadata(default(TViewModel))
        );

        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set
            {
                SetValue(ViewModelProperty, value);
                DataContext = value;
            }
        }
    }
}
