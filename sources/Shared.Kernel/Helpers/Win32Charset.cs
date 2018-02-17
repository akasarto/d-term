using System.Runtime.InteropServices;

namespace Shared.Kernel
{
	public static class Win32Charset
	{
#if ANSI
		public const CharSet BuildCharSet = CharSet.Ansi;
#else
		public const CharSet BuildCharSet = CharSet.Unicode;
#endif
	}
}
