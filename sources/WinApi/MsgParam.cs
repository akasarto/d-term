using System.Runtime.InteropServices;

namespace WinApi
{
	[StructLayout(LayoutKind.Explicit)]
	public struct MsgParam
	{
		[FieldOffset(0)]
		public uint BaseValue;

		[FieldOffset(2)]
		public ushort HOWord;

		[FieldOffset(0)]
		public ushort LOWord;
	}
}
