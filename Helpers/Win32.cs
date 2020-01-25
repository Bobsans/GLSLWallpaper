using System;
using System.Runtime.InteropServices;

namespace GLSLWallpapers.Helpers {
    public static class Win32 {
        const uint SPI_SETDESKWALLPAPER = 20;
        const uint SPIF_UPDATEINIFILE = 0x1;
        const uint SMTO_NORMAL = 0x0;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out UIntPtr lpdwResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        [DllImport("user32.dll")]
        static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        public static void SetWindowAsDesktopChild(IntPtr hwnd) {
            IntPtr progman = FindWindow("Progman", null);
            SendMessageTimeout(progman, 0x052c, UIntPtr.Zero, IntPtr.Zero, SMTO_NORMAL, 1000, out UIntPtr _);

            IntPtr workerw;
            EnumWindows((wnd, param) => {
                IntPtr shellDllView = FindWindowEx(wnd, IntPtr.Zero, "SHELLDLL_DefView", null);

                if (shellDllView != IntPtr.Zero) {
                    workerw = FindWindowEx(IntPtr.Zero, wnd, "WorkerW", null);
                    SetParent(hwnd, workerw);
                }

                return true;
            }, IntPtr.Zero);
        }

        public static void RefreshWallpaper() {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, RegistryUtils.GetCurrentWallpaper(), SPIF_UPDATEINIFILE);
        }

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        struct WindowCompositionAttributeData {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        enum WindowCompositionAttribute {
            WCA_ACCENT_POLICY = 19
        }

        enum AccentState {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        struct AccentPolicy {
            public AccentState AccentState;
            public readonly int AccentFlags;
            public readonly int GradientColor;
            public readonly int AnimationId;
        }

        public static void EnableWindowBlur(IntPtr hwnd) {
            AccentPolicy accent = new AccentPolicy();
            int accentStructSize = Marshal.SizeOf(accent);
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            WindowCompositionAttributeData data = new WindowCompositionAttributeData {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(hwnd, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }
    }
}
