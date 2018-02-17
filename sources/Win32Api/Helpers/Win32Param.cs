using System.Runtime.InteropServices;

namespace Win32Api
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Win32Param
	{
		[FieldOffset(0)]
		public uint BaseValue;

		[FieldOffset(2)]
		public ushort HOWord;

		[FieldOffset(0)]
		public ushort LOWord;
	}
}
