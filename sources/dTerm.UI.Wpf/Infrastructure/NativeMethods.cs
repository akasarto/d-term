using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace dTerm.UI.Wpf.Infrastructure
{
	[Flags]
	internal enum ShowWindowCommands
	{
		/// <summary>
		///     Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when
		///     minimizing windows from a different thread.
		/// </summary>
		SW_FORCEMINIMIZE = 11,

		/// <summary>
		///     Hides the window and activates another window.
		/// </summary>
		SW_HIDE = 0,

		/// <summary>
		///     Maximizes the specified window.
		/// </summary>
		SW_MAXIMIZE = 3,

		/// <summary>
		///     Minimizes the specified window and activates the next top-level window in the Z order.
		/// </summary>
		SW_MINIMIZE = 6,

		/// <summary>
		///     Activates and displays the window. If the window is minimized or maximized, the system restores it to its original
		///     size and position. An application should specify this flag when restoring a minimized window.
		/// </summary>
		SW_RESTORE = 9,

		/// <summary>
		///     Activates the window and displays it in its current size and position.
		/// </summary>
		SW_SHOW = 5,

		/// <summary>
		///     Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess
		///     function by the program that started the application.
		/// </summary>
		SW_SHOWDEFAULT = 10,

		/// <summary>
		///     Activates the window and displays it as a maximized window.
		/// </summary>
		SW_SHOWMAXIMIZED = 3,

		/// <summary>
		///     Activates the window and displays it as a minimized window.
		/// </summary>
		SW_SHOWMINIMIZED = 2,

		/// <summary>
		///     Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not
		///     activated.
		/// </summary>
		SW_SHOWMINNOACTIVE = 7,

		/// <summary>
		///     Displays the window in its current size and position. This value is similar to SW_SHOW, except that the window is
		///     not activated.
		/// </summary>
		SW_SHOWNA = 8,

		/// <summary>
		///     Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except that the
		///     window is not activated.
		/// </summary>
		SW_SHOWNOACTIVATE = 4,

		/// <summary>
		///     Activates and displays a window. If the window is minimized or maximized, the system restores it to its original
		///     size and position. An application should specify this flag when displaying the window for the first time.
		/// </summary>
		SW_SHOWNORMAL = 1
	}

	[Flags]
	internal enum WindowLongFlags
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

	[Flags]
	internal enum WindowExStyles
	{
		/// <summary>
		///     The window accepts drag-drop files.
		/// </summary>
		WS_EX_ACCEPTFILES = 0x00000010,

		/// <summary>
		///     Forces a top-level window onto the taskbar when the window is visible.
		/// </summary>
		WS_EX_APPWINDOW = 0x00040000,

		/// <summary>
		///     The window has a border with a sunken edge.
		/// </summary>
		WS_EX_CLIENTEDGE = 0x00000200,

		/// <summary>
		///     Paints all descendants of a window in bottom-to-top painting order using double-buffering. For more information,
		///     see Remarks. This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
		///     Windows 2000:  This style is not supported.
		/// </summary>
		WS_EX_COMPOSITED = 0x02000000,

		/// <summary>
		///     The title bar of the window includes a question mark. When the user clicks the question mark, the cursor changes to
		///     a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The
		///     child window should pass the message to the parent window procedure, which should call the WinHelp function using
		///     the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child
		///     window.
		///     WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
		/// </summary>
		WS_EX_CONTEXTHELP = 0x00000400,

		/// <summary>
		///     The window itself contains child windows that should take part in dialog box navigation. If this style is
		///     specified, the dialog manager recurses into children of this window when performing navigation operations such as
		///     handling the TAB key, an arrow key, or a keyboard mnemonic.
		/// </summary>
		WS_EX_CONTROLPARENT = 0x00010000,

		/// <summary>
		///     The window has a double border; the window can, optionally, be created with a title bar by specifying the
		///     WS_CAPTION style in the dwStyle parameter.
		/// </summary>
		WS_EX_DLGMODALFRAME = 0x00000001,

		/// <summary>
		///     The window is a layered window. This style cannot be used if the window has a class style of either CS_OWNDC or
		///     CS_CLASSDC.
		///     Windows 8:  The WS_EX_LAYERED style is supported for top-level windows and child windows. Previous Windows versions
		///     support WS_EX_LAYERED only for top-level windows.
		/// </summary>
		WS_EX_LAYERED = 0x00080000,

		/// <summary>
		///     If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the horizontal
		///     origin of the window is on the right edge. Increasing horizontal values advance to the left.
		/// </summary>
		WS_EX_LAYOUTRTL = 0x00400000,

		/// <summary>
		///     The window has generic left-aligned properties. This is the default.
		/// </summary>
		WS_EX_LEFT = 0x00000000,

		/// <summary>
		///     If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical
		///     scroll bar (if present) is to the left of the client area. For other languages, the style is ignored.
		/// </summary>
		WS_EX_LEFTSCROLLBAR = 0x00004000,

		/// <summary>
		///     The window text is displayed using left-to-right reading-order properties. This is the default.
		/// </summary>
		WS_EX_LTRREADING = 0x00000000,

		/// <summary>
		///     The window is a MDI child window.
		/// </summary>
		WS_EX_MDICHILD = 0x00000040,

		/// <summary>
		///     A top-level window created with this style does not become the foreground window when the user clicks it. The
		///     system does not bring this window to the foreground when the user minimizes or closes the foreground window.
		///     To activate the window, use the SetActiveWindow or SetForegroundWindow function.
		///     The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the
		///     WS_EX_APPWINDOW style.
		/// </summary>
		WS_EX_NOACTIVATE = 0x08000000,

		/// <summary>
		///     The window does not pass its window layout to its child windows.
		/// </summary>
		WS_EX_NOINHERITLAYOUT = 0x00100000,

		/// <summary>
		///     The child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is
		///     created or destroyed.
		/// </summary>
		WS_EX_NOPARENTNOTIFY = 0x00000004,

		/// <summary>
		///     The window does not render to a redirection surface. This is for windows that do not have visible content or that
		///     use mechanisms other than surfaces to provide their visual.
		/// </summary>
		WS_EX_NOREDIRECTIONBITMAP = 0x00200000,

		/// <summary>
		///     The window is an overlapped window.
		/// </summary>
		WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,

		/// <summary>
		///     The window is palette window, which is a modeless dialog box that presents an array of commands.
		/// </summary>
		WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,

		/// <summary>
		///     The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only
		///     if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the
		///     style is ignored.
		///     Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT
		///     style, respectively. Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON
		///     styles.
		/// </summary>
		WS_EX_RIGHT = 0x00001000,

		/// <summary>
		///     The vertical scroll bar (if present) is to the right of the client area. This is the default.
		/// </summary>
		WS_EX_RIGHTSCROLLBAR = 0x00000000,

		/// <summary>
		///     If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text
		///     is displayed using right-to-left reading-order properties. For other languages, the style is ignored.
		/// </summary>
		WS_EX_RTLREADING = 0x00002000,

		/// <summary>
		///     The window has a three-dimensional border style intended to be used for items that do not accept user input.
		/// </summary>
		WS_EX_STATICEDGE = 0x00020000,

		/// <summary>
		///     The window is intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a
		///     normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar
		///     or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not
		///     displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.
		/// </summary>
		WS_EX_TOOLWINDOW = 0x00000080,

		/// <summary>
		///     The window should be placed above all non-topmost windows and should stay above them, even when the window is
		///     deactivated. To add or remove this style, use the SetWindowPos function.
		/// </summary>
		WS_EX_TOPMOST = 0x00000008,

		/// <summary>
		///     The window should not be painted until siblings beneath the window (that were created by the same thread) have been
		///     painted. The window appears transparent because the bits of underlying sibling windows have already been painted.
		///     To achieve transparency without these restrictions, use the  SetWindowRgn function.
		/// </summary>
		WS_EX_TRANSPARENT = 0x00000020,

		/// <summary>
		///     The window has a border with a raised edge.
		/// </summary>
		WS_EX_WINDOWEDGE = 0x00000100
	}

	[Flags]
	internal enum WindowStyles
	{
		/// <summary>
		///     The window has a thin-line border.
		/// </summary>
		WS_BORDER = 0x00800000,

		/// <summary>
		///     The window has a title bar (includes the WS_BORDER style).
		/// </summary>
		WS_CAPTION = 0x00C00000,

		/// <summary>
		///     The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the
		///     WS_POPUP style.
		/// </summary>
		WS_CHILD = 0x40000000,

		/// <summary>
		///     Same as the WS_CHILD style.
		/// </summary>
		WS_CHILDWINDOW = 0x40000000,

		/// <summary>
		///     Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when
		///     creating the parent window.
		/// </summary>
		WS_CLIPCHILDREN = 0x02000000,

		/// <summary>
		///     Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message,
		///     the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be
		///     updated. If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the
		///     client area of a child window, to draw within the client area of a neighboring child window.
		/// </summary>
		WS_CLIPSIBLINGS = 0x04000000,

		/// <summary>
		///     The window is initially disabled. A disabled window cannot receive input from the user. To change this after a
		///     window has been created, use the EnableWindow function.
		/// </summary>
		WS_DISABLED = 0x08000000,

		/// <summary>
		///     The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title
		///     bar.
		/// </summary>
		WS_DLGFRAME = 0x00400000,

		/// <summary>
		///     The window is the first control of a group of controls. The group consists of this first control and all controls
		///     defined after it, up to the next control with the WS_GROUP style. The first control in each group usually has the
		///     WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus
		///     from one control in the group to the next control in the group by using the direction keys.
		///     You can turn this style on and off to change dialog box navigation. To change this style after a window has been
		///     created, use the SetWindowLong function.
		/// </summary>
		WS_GROUP = 0x00020000,

		/// <summary>
		///     The window has a horizontal scroll bar.
		/// </summary>
		WS_HSCROLL = 0x00100000,

		/// <summary>
		///     The window is initially minimized. Same as the WS_MINIMIZE style.
		/// </summary>
		WS_ICONIC = 0x20000000,

		/// <summary>
		///     The window is initially maximized.
		/// </summary>
		WS_MAXIMIZE = 0x01000000,

		/// <summary>
		///     The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must
		///     also be specified.
		/// </summary>
		WS_MAXIMIZEBOX = 0x00010000,

		/// <summary>
		///     The window is initially minimized. Same as the WS_ICONIC style.
		/// </summary>
		WS_MINIMIZE = 0x20000000,

		/// <summary>
		///     The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must
		///     also be specified.
		/// </summary>
		WS_MINIMIZEBOX = 0x00020000,

		/// <summary>
		///     The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
		/// </summary>
		WS_OVERLAPPED = 0x00000000,

		/// <summary>
		///     The window is an overlapped window. Same as the WS_TILEDWINDOW style.
		/// </summary>
		WS_OVERLAPPEDWINDOW =
			WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

		/// <summary>
		///     The windows is a pop-up window. This style cannot be used with the WS_CHILD style.
		/// </summary>
		WS_POPUP = unchecked((int)0x80000000),

		/// <summary>
		///     The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu
		///     visible.
		/// </summary>
		WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,

		/// <summary>
		///     The window has a sizing border. Same as the WS_THICKFRAME style.
		/// </summary>
		WS_SIZEBOX = 0x00040000,

		/// <summary>
		///     The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
		/// </summary>
		WS_SYSMENU = 0x00080000,

		/// <summary>
		///     The window is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key
		///     changes the keyboard focus to the next control with the WS_TABSTOP style.
		///     You can turn this style on and off to change dialog box navigation. To change this style after a window has been
		///     created, use the SetWindowLong function. For user-created windows and modeless dialogs to work with tab stops,
		///     alter the message loop to call the IsDialogMessage function.
		/// </summary>
		WS_TABSTOP = 0x00010000,

		/// <summary>
		///     The window has a sizing border. Same as the WS_SIZEBOX style.
		/// </summary>
		WS_THICKFRAME = 0x00040000,

		/// <summary>
		///     The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_OVERLAPPED
		///     style.
		/// </summary>
		WS_TILED = 0x00000000,

		/// <summary>
		///     The window is  an overlapped window. Same as the WS_OVERLAPPEDWINDOW style.
		/// </summary>
		WS_TILEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

		/// <summary>
		///     The window is initially visible.
		///     This style can be turned on and off by using the ShowWindow or SetWindowPos function.
		/// </summary>
		WS_VISIBLE = 0x10000000,

		/// <summary>
		///     The window has a vertical scroll bar.
		/// </summary>
		WS_VSCROLL = 0x00200000
	}

	public enum SysCommand
	{
		/// <summary>
		///     Closes the window.
		/// </summary>
		SC_CLOSE = 0xF060,

		/// <summary>
		///     Changes the cursor to a question mark with a pointer. If the user then clicks a control in the dialog box, the
		///     control receives a WM_HELP message.
		/// </summary>
		SC_CONTEXTHELP = 0xF180,

		/// <summary>
		///     Selects the default item; the user double-clicked the window menu.
		/// </summary>
		SC_DEFAULT = 0xF160,

		/// <summary>
		///     Activates the window associated with the application-specified hot key. The
		///     lParam parameter identifies the window to activate.
		/// </summary>
		SC_HOTKEY = 0xF150,

		/// <summary>
		///     Scrolls horizontally.
		/// </summary>
		SC_HSCROLL = 0xF080,

		/// <summary>
		///     Indicates whether the screen saver is secure.
		/// </summary>
		SCF_ISSECURE = 0x00000001,

		/// <summary>
		///     Retrieves the window menu as a result of a keystroke. For more information, see the Remarks section.
		/// </summary>
		SC_KEYMENU = 0xF100,

		/// <summary>
		///     Maximizes the window.
		/// </summary>
		SC_MAXIMIZE = 0xF030,

		/// <summary>
		///     Minimizes the window.
		/// </summary>
		SC_MINIMIZE = 0xF020,

		/// <summary>
		///     Sets the state of the display. This command supports devices that have power-saving features, such as a
		///     battery-powered personal computer.
		///     The lParam parameter can have the following values:
		///     -1 (the display is powering on)
		///     1 (the display is going to low power)
		///     2 (the display is being shut off)
		/// </summary>
		SC_MONITORPOWER = 0xF170,

		/// <summary>
		///     Retrieves the window menu as a result of a mouse click.
		/// </summary>
		SC_MOUSEMENU = 0xF090,

		/// <summary>
		///     Moves the window.
		/// </summary>
		SC_MOVE = 0xF010,

		/// <summary>
		///     Moves to the next window.
		/// </summary>
		SC_NEXTWINDOW = 0xF040,

		/// <summary>
		///     Moves to the previous window.
		/// </summary>
		SC_PREVWINDOW = 0xF050,

		/// <summary>
		///     Restores the window to its normal position and size.
		/// </summary>
		SC_RESTORE = 0xF120,

		/// <summary>
		///     Executes the screen saver application specified in the [boot] section of the System.ini file.
		/// </summary>
		SC_SCREENSAVE = 0xF140,

		/// <summary>
		///     Sizes the window.
		/// </summary>
		SC_SIZE = 0xF000,

		/// <summary>
		///     Activates the Start menu.
		/// </summary>
		SC_TASKLIST = 0xF130,

		/// <summary>
		///     Scrolls vertically.
		/// </summary>
		SC_VSCROLL = 0xF070
	}

	public enum WM
	{
		NULL = 0x0000,
		CREATE = 0x0001,
		DESTROY = 0x0002,
		MOVE = 0x0003,
		SIZE = 0x0005,
		ACTIVATE = 0x0006,
		SETFOCUS = 0x0007,
		KILLFOCUS = 0x0008,
		ENABLE = 0x000A,
		SETREDRAW = 0x000B,
		SETTEXT = 0x000C,
		GETTEXT = 0x000D,
		GETTEXTLENGTH = 0x000E,
		PAINT = 0x000F,
		CLOSE = 0x0010,
		QUERYENDSESSION = 0x0011,
		QUERYOPEN = 0x0013,
		ENDSESSION = 0x0016,
		QUIT = 0x0012,
		ERASEBKGND = 0x0014,
		SYSCOLORCHANGE = 0x0015,
		SHOWWINDOW = 0x0018,
		WININICHANGE = 0x001A,
		SETTINGCHANGE = WININICHANGE,
		DEVMODECHANGE = 0x001B,
		ACTIVATEAPP = 0x001C,
		FONTCHANGE = 0x001D,
		TIMECHANGE = 0x001E,
		CANCELMODE = 0x001F,
		SETCURSOR = 0x0020,
		MOUSEACTIVATE = 0x0021,
		CHILDACTIVATE = 0x0022,
		QUEUESYNC = 0x0023,
		GETMINMAXINFO = 0x0024,
		PAINTICON = 0x0026,
		ICONERASEBKGND = 0x0027,
		NEXTDLGCTL = 0x0028,
		SPOOLERSTATUS = 0x002A,
		DRAWITEM = 0x002B,
		MEASUREITEM = 0x002C,
		DELETEITEM = 0x002D,
		VKEYTOITEM = 0x002E,
		CHARTOITEM = 0x002F,
		SETFONT = 0x0030,
		GETFONT = 0x0031,
		SETHOTKEY = 0x0032,
		GETHOTKEY = 0x0033,
		QUERYDRAGICON = 0x0037,
		COMPAREITEM = 0x0039,
		GETOBJECT = 0x003D,
		COMPACTING = 0x0041,
		COMMNOTIFY = 0x0044 /* no longer suported */,
		WINDOWPOSCHANGING = 0x0046,
		WINDOWPOSCHANGED = 0x0047,
		POWER = 0x0048,
		COPYDATA = 0x004A,
		CANCELJOURNAL = 0x004B,
		NOTIFY = 0x004E,
		INPUTLANGCHANGEREQUEST = 0x0050,
		INPUTLANGCHANGE = 0x0051,
		TCARD = 0x0052,
		HELP = 0x0053,
		USERCHANGED = 0x0054,
		NOTIFYFORMAT = 0x0055,
		CONTEXTMENU = 0x007B,
		STYLECHANGING = 0x007C,
		STYLECHANGED = 0x007D,
		DISPLAYCHANGE = 0x007E,
		GETICON = 0x007F,
		SETICON = 0x0080,
		NCCREATE = 0x0081,
		NCDESTROY = 0x0082,
		NCCALCSIZE = 0x0083,
		NCHITTEST = 0x0084,
		NCPAINT = 0x0085,
		NCACTIVATE = 0x0086,
		GETDLGCODE = 0x0087,
		SYNCPAINT = 0x0088,
		NCMOUSEMOVE = 0x00A0,
		NCLBUTTONDOWN = 0x00A1,
		NCLBUTTONUP = 0x00A2,
		NCLBUTTONDBLCLK = 0x00A3,
		NCRBUTTONDOWN = 0x00A4,
		NCRBUTTONUP = 0x00A5,
		NCRBUTTONDBLCLK = 0x00A6,
		NCMBUTTONDOWN = 0x00A7,
		NCMBUTTONUP = 0x00A8,
		NCMBUTTONDBLCLK = 0x00A9,
		NCXBUTTONDOWN = 0x00AB,
		NCXBUTTONUP = 0x00AC,
		NCXBUTTONDBLCLK = 0x00AD,
		INPUT_DEVICE_CHANGE = 0x00FE,
		INPUT = 0x00FF,
		KEYFIRST = 0x0100,
		KEYDOWN = 0x0100,
		KEYUP = 0x0101,
		CHAR = 0x0102,
		DEADCHAR = 0x0103,
		SYSKEYDOWN = 0x0104,
		SYSKEYUP = 0x0105,
		SYSCHAR = 0x0106,
		SYSDEADCHAR = 0x0107,
		UNICHAR = 0x0109,
		KEYLAST = 0x0109,
		IME_STARTCOMPOSITION = 0x010D,
		IME_ENDCOMPOSITION = 0x010E,
		IME_COMPOSITION = 0x010F,
		IME_KEYLAST = 0x010F,
		INITDIALOG = 0x0110,
		COMMAND = 0x0111,
		SYSCOMMAND = 0x0112,
		TIMER = 0x0113,
		HSCROLL = 0x0114,
		VSCROLL = 0x0115,
		INITMENU = 0x0116,
		INITMENUPOPUP = 0x0117,
		GESTURE = 0x0119,
		GESTURENOTIFY = 0x011A,
		MENUSELECT = 0x011F,
		MENUCHAR = 0x0120,
		ENTERIDLE = 0x0121,
		MENURBUTTONUP = 0x0122,
		MENUDRAG = 0x0123,
		MENUGETOBJECT = 0x0124,
		UNINITMENUPOPUP = 0x0125,
		MENUCOMMAND = 0x0126,
		CHANGEUISTATE = 0x0127,
		UPDATEUISTATE = 0x0128,
		QUERYUISTATE = 0x0129,
		CTLCOLORMSGBOX = 0x0132,
		CTLCOLOREDIT = 0x0133,
		CTLCOLORLISTBOX = 0x0134,
		CTLCOLORBTN = 0x0135,
		CTLCOLORDLG = 0x0136,
		CTLCOLORSCROLLBAR = 0x0137,
		CTLCOLORSTATIC = 0x0138,
		MOUSEFIRST = 0x0200,
		MOUSEMOVE = 0x0200,
		LBUTTONDOWN = 0x0201,
		LBUTTONUP = 0x0202,
		LBUTTONDBLCLK = 0x0203,
		RBUTTONDOWN = 0x0204,
		RBUTTONUP = 0x0205,
		RBUTTONDBLCLK = 0x0206,
		MBUTTONDOWN = 0x0207,
		MBUTTONUP = 0x0208,
		MBUTTONDBLCLK = 0x0209,
		MOUSEWHEEL = 0x020A,
		XBUTTONDOWN = 0x020B,
		XBUTTONUP = 0x020C,
		XBUTTONDBLCLK = 0x020D,
		MOUSEHWHEEL = 0x020E,
		MOUSELAST = 0x020E,
		PARENTNOTIFY = 0x0210,
		ENTERMENULOOP = 0x0211,
		EXITMENULOOP = 0x0212,
		NEXTMENU = 0x0213,
		SIZING = 0x0214,
		CAPTURECHANGED = 0x0215,
		MOVING = 0x0216,
		POWERBROADCAST = 0x0218,
		DEVICECHANGE = 0x0219,
		MDICREATE = 0x0220,
		MDIDESTROY = 0x0221,
		MDIACTIVATE = 0x0222,
		MDIRESTORE = 0x0223,
		MDINEXT = 0x0224,
		MDIMAXIMIZE = 0x0225,
		MDITILE = 0x0226,
		MDICASCADE = 0x0227,
		MDIICONARRANGE = 0x0228,
		MDIGETACTIVE = 0x0229,
		MDISETMENU = 0x0230,
		ENTERSIZEMOVE = 0x0231,
		EXITSIZEMOVE = 0x0232,
		DROPFILES = 0x0233,
		MDIREFRESHMENU = 0x0234,
		POINTERDEVICECHANGE = 0x238,
		POINTERDEVICEINRANGE = 0x239,
		POINTERDEVICEOUTOFRANGE = 0x23A,
		TOUCH = 0x0240,
		NCPOINTERUPDATE = 0x0241,
		NCPOINTERDOWN = 0x0242,
		NCPOINTERUP = 0x0243,
		POINTERUPDATE = 0x0245,
		POINTERDOWN = 0x0246,
		POINTERUP = 0x0247,
		POINTERENTER = 0x0249,
		POINTERLEAVE = 0x024A,
		POINTERACTIVATE = 0x024B,
		POINTERCAPTURECHANGED = 0x024C,
		TOUCHHITTESTING = 0x024D,
		POINTERWHEEL = 0x024E,
		POINTERHWHEEL = 0x024F,
		IME_SETCONTEXT = 0x0281,
		IME_NOTIFY = 0x0282,
		IME_CONTROL = 0x0283,
		IME_COMPOSITIONFULL = 0x0284,
		IME_SELECT = 0x0285,
		IME_CHAR = 0x0286,
		IME_REQUEST = 0x0288,
		IME_KEYDOWN = 0x0290,
		IME_KEYUP = 0x0291,
		MOUSEHOVER = 0x02A1,
		MOUSELEAVE = 0x02A3,
		NCMOUSEHOVER = 0x02A0,
		NCMOUSELEAVE = 0x02A2,
		WTSSESSION_CHANGE = 0x02B1,
		TABLET_FIRST = 0x02c0,
		TABLET_LAST = 0x02df,
		DPICHANGED = 0x02E0,
		CUT = 0x0300,
		COPY = 0x0301,
		PASTE = 0x0302,
		CLEAR = 0x0303,
		UNDO = 0x0304,
		RENDERFORMAT = 0x0305,
		RENDERALLFORMATS = 0x0306,
		DESTROYCLIPBOARD = 0x0307,
		DRAWCLIPBOARD = 0x0308,
		PAINTCLIPBOARD = 0x0309,
		VSCROLLCLIPBOARD = 0x030A,
		SIZECLIPBOARD = 0x030B,
		ASKCBFORMATNAME = 0x030C,
		CHANGECBCHAIN = 0x030D,
		HSCROLLCLIPBOARD = 0x030E,
		QUERYNEWPALETTE = 0x030F,
		PALETTEISCHANGING = 0x0310,
		PALETTECHANGED = 0x0311,
		HOTKEY = 0x0312,
		PRINT = 0x0317,
		PRINTCLIENT = 0x0318,
		APPCOMMAND = 0x0319,
		THEMECHANGED = 0x031A,
		CLIPBOARDUPDATE = 0x031D,
		DWMCOMPOSITIONCHANGED = 0x031E,
		DWMNCRENDERINGCHANGED = 0x031F,
		DWMCOLORIZATIONCOLORCHANGED = 0x0320,
		DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
		DWMSENDICONICTHUMBNAIL = 0x0323,
		DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,
		GETTITLEBARINFOEX = 0x033F,
		HANDHELDFIRST = 0x0358,
		HANDHELDLAST = 0x035F,
		AFXFIRST = 0x0360,
		AFXLAST = 0x037F,
		PENWINFIRST = 0x0380,
		PENWINLAST = 0x038F,
		USER = 0x0400,
		APP = 0x8000,

		// Custom Messages
		APPViewHighlight = (APP + 0x1)
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Win32Param
	{
		[FieldOffset(0)]
		public uint BaseValue;

		[FieldOffset(2)]
		public ushort HIWord;

		[FieldOffset(0)]
		public ushort LOWord;
	}

	internal static class Properties
	{
#if ANSI
		public const CharSet BuildCharSet = CharSet.Ansi;
#else
		public const CharSet BuildCharSet = CharSet.Unicode;
#endif
	}

	internal static class NativeMethods
	{
		private const string user32Dll = "user32.dll";

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern bool DestroyWindow(IntPtr hwnd);

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern bool CloseWindow(IntPtr hwnd);

		[DllImport(user32Dll, CharSet = Properties.BuildCharSet)]
		internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport(user32Dll, ExactSpelling = true)]
		internal static extern bool IsWindowVisible(IntPtr hwnd);

		[DllImport(user32Dll, ExactSpelling = true, CharSet = Properties.BuildCharSet)]
		internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport(user32Dll, ExactSpelling = true)]
		internal static extern bool ShowWindow(IntPtr hwnd, ShowWindowCommands nCmdShow);

		[DllImport(user32Dll, CharSet = Properties.BuildCharSet)]
		internal static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport(user32Dll, ExactSpelling = true)]
		internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		internal static IntPtr GetWindowLongPtr(IntPtr hwnd, WindowLongFlags nIndex)
		{
			if (IntPtr.Size > 4)
			{
				return GetWindowLongPtr(hwnd, (int)nIndex);
			}

			return new IntPtr(GetWindowLong(hwnd, (int)nIndex));
		}

		internal static IntPtr SetWindowLongPtr(IntPtr hwnd, WindowLongFlags nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size > 4)
			{
				return SetWindowLongPtr(hwnd, (int)nIndex, dwNewLong);
			}

			return new IntPtr(SetWindowLong(hwnd, (int)nIndex, dwNewLong.ToInt32()));
		}

		[DllImport(user32Dll, CharSet = Properties.BuildCharSet)]
		private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[DllImport(user32Dll, CharSet = Properties.BuildCharSet, EntryPoint = "GetWindowLongPtr")]
		private static extern IntPtr GetWindowLongPtr(IntPtr hwnd, int nIndex);

		[DllImport(user32Dll, CharSet = Properties.BuildCharSet)]
		private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[DllImport(user32Dll, CharSet = Properties.BuildCharSet, EntryPoint = "SetWindowLongPtr")]
		private static extern IntPtr SetWindowLongPtr(IntPtr hwnd, int nIndex, IntPtr dwNewLong);

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern bool SetForegroundWindow(IntPtr hwnd);
	}
}
