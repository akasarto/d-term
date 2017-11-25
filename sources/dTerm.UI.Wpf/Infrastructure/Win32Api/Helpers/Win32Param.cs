using System.Runtime.InteropServices;

namespace dTerm.UI.Wpf.Infrastructure
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct Win32Param
	{
		[FieldOffset(0)]
		public uint BaseValue;

		[FieldOffset(2)]
		public ushort HIWord;

		[FieldOffset(0)]
		public ushort LOWord;
	}
}
