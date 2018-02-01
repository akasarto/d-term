using System;

namespace dTerm.WinNative
{
	[Flags]
	public enum WindowLongFlags
	{
		/// <summary>
		///     Retrieves the extended window styles.
		/// </summary>
		GWL_EXSTYLE = -20,

		/// <summary>
		///     Retrieves a handle to the application instance.
		/// </summary>
		GWLP_HINSTANCE = -6,

		/// <summary>
		///     Retrieves a handle to the parent window, if there is one.
		/// </summary>
		GWLP_HWNDPARENT = -8,

		/// <summary>
		///     Retrieves the identifier of the window.
		/// </summary>
		GWLP_ID = -12,

		/// <summary>
		///     Retrieves the window styles.
		/// </summary>
		GWL_STYLE = -16,

		/// <summary>
		///     Retrieves the user data associated with the window. This data is intended for use by the application that created
		///     the window. Its value is initially zero.
		/// </summary>
		GWLP_USERDATA = -21,

		/// <summary>
		///     Retrieves the pointer to the window procedure, or a handle representing the pointer to the window procedure. You
		///     must use the CallWindowProc function to call the window procedure.
		/// </summary>
		GWLP_WNDPROC = -4,

		/// <summary>
		///     Retrieves the pointer to the dialog box procedure, or a handle representing the pointer to the dialog box
		///     procedure. You must use the CallWindowProc function to call the dialog box procedure.
		///     Note: Should be DWLP_MSGRESULT + sizeof(LRESULT)
		/// </summary>
		DWLP_DLGPROC = 0x4,

		/// <summary>
		///     Retrieves the return value of a message processed in the dialog box procedure.
		/// </summary>
		DWLP_MSGRESULT = 0,

		/// <summary>
		///     Retrieves extra information private to the application, such as handles or pointers.
		///     Note: Should be DWLP_DLGPROC + sizeof(DLGPROC)
		/// </summary>
		DWLP_USER = 0x8
	}
}
