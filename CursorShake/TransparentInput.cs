using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace CursorShake
{
    public static class TransparentInput
    {
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void ToTransparentWindow(this Window x)
        {
            x.SourceInitialized += delegate
                {
                    // Get this window's handle
                    IntPtr hwnd = new WindowInteropHelper(x).Handle;

                    // Change the extended window style to include WS_EX_TRANSPARENT
                    int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

                    _ = SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
                };
        }
    }
}
