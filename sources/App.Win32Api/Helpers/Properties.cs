using System.Runtime.InteropServices;

namespace App.Win32Api
{
	public static class Properties
	{
#if ANSI
		public const CharSet BuildCharSet = CharSet.Ansi;
#else
		public const CharSet BuildCharSet = CharSet.Unicode;
#endif
	}
}
