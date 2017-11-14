namespace dTerm.Core.Events
{
	public interface IEventMessageHandler<T> where T : IEventMessage
	{
		void Handle(T args);
	}
}
