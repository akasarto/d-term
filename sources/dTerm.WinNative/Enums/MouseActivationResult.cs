namespace dTerm.WinNative
{
	public enum MouseActivationResult
	{
		/// <summary>
		///     Activates the window, and does not discard the mouse message.
		/// </summary>
		MA_ACTIVATE = 1,

		/// <summary>
		///     Activates the window, and discards the mouse message.
		/// </summary>
		MA_ACTIVATEANDEAT = 2,

		/// <summary>
		///     Does not activate the window, and does not discard the mouse message.
		/// </summary>
		MA_NOACTIVATE = 3,

		/// <summary>
		///     Does not activate the window, but discards the mouse message.
		/// </summary>
		MA_NOACTIVATEANDEAT = 4
	}
}
