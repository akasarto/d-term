using System.Runtime.InteropServices;

namespace App.Consoles.Service.Win32Api
{
	internal static class Properties
	{
#if ANSI
		public const CharSet BuildCharSet = CharSet.Ansi;
#else
		public const CharSet BuildCharSet = CharSet.Unicode;
#endif
	}
}
