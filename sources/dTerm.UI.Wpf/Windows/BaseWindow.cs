using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace dTerm.UI.Wpf.Windows
{
    public abstract class BaseWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(propertyName)
                );

                return true;
            }

            return false;
        }
    }
}
