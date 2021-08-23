using ReactiveUI;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;

namespace dTerm.UI.Wpf.Views
{
    public static class ValidadationBinder
    {
        private static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> property) where T : class
        {
            if (property.Body is MemberExpression member && member.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo.Name;
            }

            throw new ArgumentException("Invalid property expression.");
        }

        public static IDisposable BindValidationError<TView, TViewModel, TValidatableObject, TProperty>(this TView view, TViewModel viewModel, Expression<Func<TViewModel, TValidatableObject>> objectToValidateName, Expression<Func<TValidatableObject, TProperty>> propertyToValidate, Func<TView, DependencyObject> uiElementDelegate) where TViewModel : class where TView : IViewFor<TViewModel> where TValidatableObject : class, INotifyDataErrorInfo
        {
            //string lastError = null;

            var propertyToValidateName = propertyToValidate.GetPropertyName();

            return viewModel.WhenAnyValue(objectToValidateName).StartWith(objectToValidateName.Compile().Invoke(viewModel)).Do(objectToValidate =>
            {
                var uiElement = uiElementDelegate.Invoke(view);

                if (objectToValidate == null)
                {
                    //ValidationHelper.ClearValidationErrors(uiElement);

                    return;
                }

                //ValidateProperty(objectToValidate, propertyToValidateName, uiElement, ref lastError);

            }).Select(objectToValidate =>
            {
                return objectToValidate != null
                    ? Observable.FromEventPattern<DataErrorsChangedEventArgs>(objectToValidate, nameof(objectToValidate.ErrorsChanged))
                    : Observable.Empty<EventPattern<DataErrorsChangedEventArgs>>();

            }).Switch().Subscribe(eventArgs =>
            {
                if (eventArgs.EventArgs.PropertyName == propertyToValidateName)
                {
                    var objectToValidate = (INotifyDataErrorInfo)eventArgs.Sender;
                    var uiElement = uiElementDelegate.Invoke(view);

                    //ValidateProperty(objectToValidate, propertyToValidateName, uiElement, ref lastError);
                }
            });
        }
    }
}
