﻿using System.Runtime.InteropServices;

namespace dTerm.WinNative
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