namespace UI.Wpf.Consoles
{
	public class ArrangeChangedMessage
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ArrangeChangedMessage(ArrangeOption newArrange)
		{
			NewArrange = newArrange;
		}

		public ArrangeOption NewArrange { get; private set; }
	}
}
