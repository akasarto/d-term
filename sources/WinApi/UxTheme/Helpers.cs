using System;
using System.Runtime.InteropServices;
using WinApi.Core;

namespace WinApi.UxTheme
{
    public static class UxThemeHelpers
    {
        public static unsafe HResult SetWindowThemeNonClientAttributes(IntPtr hwnd,
            WindowThemeNcAttributeFlags mask,
            WindowThemeNcAttributeFlags attributes)
        {
            var opts = new WindowThemeAttributeOptions
            {
                Mask = (uint) mask,
                Flags = (uint) attributes
            };
            return UxThemeMethods.SetWindowThemeAttribute(hwnd, WindowThemeAttributeType.WTA_NONCLIENT,
                new IntPtr(&opts), (uint) Marshal.SizeOf<WindowThemeAttributeOptions>());
        }
    }
}