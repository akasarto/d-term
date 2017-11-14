using System;
using System.Collections.Generic;

namespace dTerm.Core.Events
{
	public class EventBus
	{
		[ThreadStatic]
		private static List<Delegate> _handlers;

		public static void Dispose()
		{
			_handlers = null;
		}

		public static void Publish<T>(T message) where T : IEventMessage
		{
			if (_handlers != null)
			{
				foreach (var handler in _handlers)
				{
					if (handler is Action<T>)
					{
						(handler as Action<T>)?.Invoke(message);
					}
				}
			}
		}

		public static void Subscribe<T>(Action<T> handler) where T : IEventMessage
		{
			if (_handlers == null)
			{
				_handlers = new List<Delegate>();
			}

			_handlers.Add(handler);
		}
	}
}
