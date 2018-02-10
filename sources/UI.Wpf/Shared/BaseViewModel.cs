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
	/// Base class for all view models.
	/// </summary>
	public abstract class BaseViewModel : ReactiveObject, INotifyDataErrorInfo
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
		/// <returns></returns>
		public IEnumerable GetErrors(string propertyName)
		{
			if (_modelErrors.ContainsKey(propertyName))
			{
				return _modelErrors[propertyName].FirstOrDefault() ?? string.Empty;
			}

			return null;
		}

		/// <summary>
		/// Set the errors based on the given validation result.
		/// </summary>
		/// <param name="validationResult">The validation result returned by the validation engine.</param>
		/// <returns><c>True</c> if any errors were set. Otherwise, <c>false</c>.</returns>
		protected bool SetErrors(ValidationResult validationResult)
		{
			var newErrors = validationResult.Errors.Select(e => e.PropertyName).ToList();
			var oldErrors = _modelErrors.Keys.ToList();

			var removeErrors = oldErrors.Except(newErrors);

			foreach (var removeError in removeErrors)
			{
				_modelErrors[removeError].Clear();
				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(removeError));
			}

			foreach(var addError in newErrors)
			{
				var errors = validationResult.Errors.Where(e => e.PropertyName.Equals(addError)).ToList();

				foreach (var error in errors)
				{
					var message = error.ErrorMessage;

					_modelErrors.AddOrUpdate(addError, new List<string>() { message }, (key, messages) =>
					{
						if (!messages.Contains(message))
						{
							messages.Add(message);
						}

						return messages;
					});
				}

				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(addError));
			}

			return HasErrors;
		}
	}
}
