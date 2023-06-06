using System.Runtime.InteropServices;

namespace GLSLWallpaper;

public static partial class Win32 {
    const uint SPI_SETDESKWALLPAPER = 20;
    const uint SPIF_UPDATEINIFILE = 0x1;
    const uint SMTO_NORMAL = 0x0;
    const uint WH_MOUSE_LL = 0xE;

    [LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    private static partial nint FindWindowW(string lpClassName, string? lpWindowName);

    [LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    private static partial nint FindWindowExW(nint hwndParent, nint hwndChildAfter, string lpszClass, string? lpszWindow);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial void EnumWindows(EnumWindowsProc lpEnumFunc, nint lParam);

    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial void SetParent(nint hWndChild, nint hWndNewParent);

    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial void SendMessageTimeoutW(nint hWnd, uint msg, nuint wParam, nint lParam, uint fuFlags, uint uTimeout, out nuint lpdwResult);

    [LibraryImport("user32.dll", StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial void SystemParametersInfoW(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetCursorPos(out Point point);

    [LibraryImport("user32.dll")]
    private static partial int GetAsyncKeyState(VirtualKey vKeys);

    public static void SetWindowAsDesktopChild(nint hwnd) {
        nint progmanHwnd = FindWindowW("Progman", null);
        SendMessageTimeoutW(progmanHwnd, 0x052c, nuint.Zero, nint.Zero, SMTO_NORMAL, 1000, out nuint _);

        EnumWindows((wnd, _) => {
            nint shellDllView = FindWindowExW(wnd, nint.Zero, "SHELLDLL_DefView", null);

            if (shellDllView != nint.Zero) {
                nint desktopHwnd = FindWindowExW(nint.Zero, wnd, "WorkerW", null);
                SetParent(hwnd, desktopHwnd);
            }

            return true;
        }, nint.Zero);
    }

    public static void RefreshWallpaper() {
        SystemParametersInfoW(SPI_SETDESKWALLPAPER, 0, RegistryHelper.GetCurrentWallpaper()!, SPIF_UPDATEINIFILE);
    }

    public static Point GetMousePosition() {
        GetCursorPos(out Point point);
        return point;
    }

    public static MouseState GetMouseState() {
        return new MouseState {
            Position = GetMousePosition(),
            Left = (GetAsyncKeyState(VirtualKey.VK_LBUTTON) & 0x8000) != 0,
            Right = (GetAsyncKeyState(VirtualKey.VK_RBUTTON) & 0x8000) != 0
        };
    }

    delegate bool EnumWindowsProc(nint hWnd, nint lParam);

    [StructLayout(LayoutKind.Sequential)]
    public struct Point {
        public readonly int X;
        public readonly int Y;
    }

    public struct MouseState {
        public Point Position;
        public bool Left;
        public bool Right;
    }

    public enum VirtualKey {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_MBUTTON = 0x04
    }
}
