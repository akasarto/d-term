using System.Runtime.InteropServices;

namespace App.Consoles.Service.Win32Api
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
