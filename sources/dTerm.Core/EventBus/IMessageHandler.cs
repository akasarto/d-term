namespace dTerm.Core.DataBus
{
	public interface IMessageHandler<T> where T : IMessage
	{
		void Handle(T args);
	}
}
