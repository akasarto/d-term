﻿using System.Windows;

namespace dTerm.UI.Wpf.Views
{
    public abstract class BindableWindow<TViewModel> : Window where TViewModel : class
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(TViewModel),
            typeof(BindableWindow<TViewModel>),
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
