using System.Runtime.InteropServices;

namespace dTerm.Core
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Win32LWParams
	{
		[FieldOffset(0)]
		public uint Param;

		[FieldOffset(0)]
		public ushort Low;

		[FieldOffset(2)]
		public ushort High;
	}

}
