using FluentValidation.Results;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace UI.Wpf
{
	public abstract class ReactiveObjectWithValidation : ReactiveObject, INotifyDataErrorInfo
	{
		private ConcurrentDictionary<string, List<string>> _modelErrors = new ConcurrentDictionary<string, List<string>>();

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		public bool HasErrors => _modelErrors.Values.Any(v => v.Count > 0);

		public IEnumerable GetErrors(string propertyName)
		{
			if (_modelErrors.ContainsKey(propertyName))
			{
				return _modelErrors[propertyName]?.ToList();
			}

			return null;
		}

		public void MergeErrors(IEnumerable<ValidationFailure> validationFailures)
		{
			var newInvalidProperties = validationFailures.Select(e => e.PropertyName).ToList();
			var oldInvalidProperties = _modelErrors.Keys.ToList();

			var validProperties = oldInvalidProperties.Except(newInvalidProperties);

			foreach (var validProperty in validProperties)
			{
				_modelErrors[validProperty].Clear();
				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(validProperty));
			}

			foreach (var invalidProperty in newInvalidProperties)
			{
				var failures = validationFailures.Where(e => e.PropertyName.Equals(invalidProperty)).ToList();

				foreach (var failure in failures)
				{
					var errorMessage = failure.ErrorMessage;

					_modelErrors.AddOrUpdate(invalidProperty, new List<string>() { errorMessage }, (key, messages) =>
					{
						if (!messages.Contains(errorMessage))
						{
							messages.Add(errorMessage);
						}

						return messages;
					});
				}

				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(invalidProperty));
			}
		}
	}
}
