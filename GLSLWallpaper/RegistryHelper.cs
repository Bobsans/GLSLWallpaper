using System;
using System.Text;
using Microsoft.Win32;

namespace GLSLWallpaper;

public static class RegistryHelper {
    static void Set(RegistryKey root, string path, string key, object value) {
        using RegistryKey registryKey = root.CreateSubKey(path, true);
        registryKey.SetValue(key, value);
    }

    static T? Get<T>(RegistryKey root, string path, string key, T? defaultValue = default) {
        using RegistryKey? registryKey = root.OpenSubKey(path);
        object? value = registryKey?.GetValue(key);
        return value != null ? (T)Convert.ChangeType(value, typeof(T)) : defaultValue;
    }

    public static void SetConfig(string key, object value) {
        Set(Registry.CurrentUser, $"Software\\{Identity.NAME}", key, value);
    }

    public static T GetConfig<T>(string key, T defaultValue) {
        return Get<T>(Registry.CurrentUser, $"Software\\{Identity.NAME}", key) ?? defaultValue;
    }

    public static string? GetCurrentWallpaper() {
        byte[]? path = Get<byte[]>(Registry.CurrentUser, "Control Panel\\Desktop", "TranscodedImageCache");
        return path != null ? Encoding.Unicode.GetString(path[24..]).TrimEnd('\0') : null;
    }
}
