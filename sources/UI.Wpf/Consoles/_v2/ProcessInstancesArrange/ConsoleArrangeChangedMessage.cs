namespace UI.Wpf.Consoles
{
	public class ConsoleArrangeChangedMessage
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleArrangeChangedMessage(ConsoleArrangeOption newArrange)
		{
			NewArrange = newArrange;
		}

		/// <summary>
		/// Gets the new arrange.
		/// </summary>
		public ConsoleArrangeOption NewArrange { get; private set; }
	}
}
