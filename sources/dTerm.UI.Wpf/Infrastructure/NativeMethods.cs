using System;
using System.Runtime.InteropServices;

namespace dTerm.UI.Wpf.Infrastructure
{
	public static class NativeMethods
	{
		[DllImport("user32.dll", ExactSpelling = true)]
		internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
	}
}
