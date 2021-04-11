using ReactiveUI;
using Splat;
using System;
using System.Windows;
using System.Windows.Controls;

namespace dTerm.UI.Wpf
{
    public static class LocalExtensions
    {
        public static IViewFor<TViewModel> GetView<TViewModel>(this TViewModel viewModel) where TViewModel : ReactiveObject
        {
            if (Locator.Current.GetService<IViewFor<TViewModel>>() is var view)
            {
                if (view.ViewModel is null)
                {
                    view.ViewModel = viewModel;
                }

                return view;
            }

            return null;
        }

        public static Window GetWindow<TViewModel>(this TViewModel viewModel) where TViewModel : ReactiveObject
        {
            if (Locator.Current.GetService<IViewFor<TViewModel>>() is var view)
            {
                if (view.ViewModel is null)
                {
                    view.ViewModel = viewModel;
                }

                if (view is not Window)
                {
                    throw new InvalidOperationException("Underlying view is not a window.", new Exception(nameof(LocalExtensions)));
                }

                return (Window)view;
            }

            return null;
        }

        public static UserControl GetUserControl<TViewModel>(this TViewModel viewModel) where TViewModel : ReactiveObject
        {
            if (Locator.Current.GetService<IViewFor<TViewModel>>() is var view)
            {
                if (view.ViewModel is null)
                {
                    view.ViewModel = viewModel;
                }

                if (view is not UserControl)
                {
                    throw new InvalidOperationException("Underlying view is not an user control.", new Exception(nameof(LocalExtensions)));
                }

                return (UserControl)view;
            }

            return null;
        }
    }
}
