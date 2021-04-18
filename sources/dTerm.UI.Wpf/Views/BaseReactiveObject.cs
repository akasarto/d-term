using FluentValidation;
using FluentValidation.Results;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace dTerm.UI.Wpf.Views
{
    public abstract class BaseReactiveObject : ReactiveObject, INotifyDataErrorInfo
    {
        private readonly ConcurrentDictionary<string, List<string>> _modelErrors = new();

        public bool HasErrors => _modelErrors.Values.Any(v => v.Count > 0);

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
    }
}
