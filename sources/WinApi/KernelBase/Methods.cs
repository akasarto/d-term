using System;
using System.Runtime.InteropServices;

namespace WinApi.KernelBase
{
    public static class Kernel32Methods
    {
        public const string LibraryName = "kernelbase";

        [DllImport(LibraryName, ExactSpelling = true)]
        public static extern bool CompareObjectHandles(IntPtr hFirstObjectHandle, IntPtr hSecondObjectHandle);
    }
}