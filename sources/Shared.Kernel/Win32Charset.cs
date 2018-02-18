using System.Runtime.InteropServices;

namespace Shared.Kernel
{
	public static class Win32Charset
	{
#if ANSI
		public const CharSet Current = CharSet.Ansi;
#else
		public const CharSet Current = CharSet.Unicode;
#endif
	}
}