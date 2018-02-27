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
	/// <summary>
	/// Reactive base class that supports data validation.
	/// <seealso cref="Processes.ProcessViewModel"/>
	/// </summary>
	public abstract class ReactiveObjectWithValidation : ReactiveObject, INotifyDataErrorInfo
	{
		private ConcurrentDictionary<string, List<string>> _modelErrors = new ConcurrentDictionary<string, List<string>>();

		/// <summary>
		/// Notifies subscribers about any property erros.
		/// </summary>
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		/// <summary>
		/// Flags when there is any error in the model.
		/// </summary>
		public bool HasErrors => _modelErrors.Values.Any(v => v.Count > 0);

		/// <summary>
		/// Get the existing erros for the given property.
		/// </summary>
		/// <param name="propertyName">The property to get the errors from.</param>
		/// <returns>An errors <see cref="IEnumerable"/>.</returns>
		public IEnumerable GetErrors(string propertyName)
		{
			if (_modelErrors.ContainsKey(propertyName))
			{
				return _modelErrors[propertyName]?.ToList();
			}

			return null;
		}

		/// <summary>
		/// Merge the failures of this instance.
		/// </summary>
		/// <param name="validationFailures">The validation failures to be merged.</param>
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
