using System.Runtime.InteropServices;

namespace dTerm.Core
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Win32Param
	{
		[FieldOffset(0)]
		public uint BaseValue;

		[FieldOffset(2)]
		public ushort HighWord;

		[FieldOffset(0)]
		public ushort LowWord;
	}

}
