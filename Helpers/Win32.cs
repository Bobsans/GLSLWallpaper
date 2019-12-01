using System;
using System.Runtime.InteropServices;

namespace GLSLWallpapers.Helpers {
    public static class Win32 {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout, out UIntPtr lpdwResult);

        [Flags]
        enum SendMessageTimeoutFlags : uint {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        public static IntPtr SetWindowAsDesktopChild(IntPtr hwnd) {
//            IntPtr progman = FindWindow("Progman", null);
//            SendMessageTimeout(progman, 0x052c, UIntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 100, out UIntPtr _);

            IntPtr workerw = IntPtr.Zero;
            EnumWindows((wnd, param) => {
                IntPtr w = FindWindowEx(wnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (w != IntPtr.Zero) {
                    workerw = FindWindowEx(IntPtr.Zero, wnd, "WorkerW", null);
                    SetParent(hwnd, workerw);
                }

                return true;
            }, IntPtr.Zero);

            return workerw;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);

        static UInt32 SPI_SETDESKWALLPAPER = 20;
        static UInt32 SPIF_UPDATEINIFILE = 0x1;

        public static void SetImage(string filename) {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE);
        }
    }
}