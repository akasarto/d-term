using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace dTerm.UI.Wpf.Views
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly ConcurrentDictionary<string, List<string>> _modelErrors = new();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasErrors => _modelErrors.Values.Any(v => v.Count > 0);

        public IEnumerable GetErrors(string propertyName)
        {
            if (_modelErrors.ContainsKey(propertyName))
            {
                return _modelErrors[propertyName]?.ToList();
            }

            return null;
        }

        public void NotifyErrors(IEnumerable<ValidationFailure> validationFailures)
        {
            var oldFailedProperties = _modelErrors.Keys.ToList();
            var newFailedProperties = validationFailures.Select(item => item.PropertyName).ToList();

            foreach (var successfulProperty in oldFailedProperties)
            {
                _modelErrors[successfulProperty].Clear();

                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(successfulProperty));
            }

            foreach (var failedProperty in newFailedProperties)
            {
                var failures = validationFailures.Where(item => item.PropertyName.Equals(failedProperty)).ToList();

                foreach (var failure in failures)
                {
                    var errorMessage = failure.ErrorMessage;

                    _modelErrors.AddOrUpdate(failedProperty, new List<string>() { errorMessage }, (key, messages) =>
                    {
                        if (!messages.Contains(errorMessage))
                        {
                            messages.Add(errorMessage);
                        }

                        return messages;
                    });
                }

                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(failedProperty));
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T currentFieldValue, T newFieldValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(currentFieldValue, newFieldValue))
            {
                currentFieldValue = newFieldValue;

                OnPropertyChanged(propertyName);

                return true;
            }

            return false;
        }
    }
}
