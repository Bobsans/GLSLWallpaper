using System;
using System.Text;
using Microsoft.Win32;

namespace GLSLWallpapers.Helpers {
    public static class RegistryUtils {
        static void Set(RegistryKey root, string path, string key, object value) {
            using (RegistryKey appKey = root.CreateSubKey(path, true)) {
                appKey?.SetValue(key, value);
            }
        }

        static T Get<T>(RegistryKey root, string path, string key, T _default) {
            using (RegistryKey appKey = root.OpenSubKey(path)) {
                object value = appKey?.GetValue(key);
                return value != null ? typeof(T) == typeof(object) ? (T)value : (T)Convert.ChangeType(value, typeof(T)) : _default;
            }
        }

        public static void SetConfig(string key, object value) {
            Set(Registry.CurrentUser, $"Software\\{Reference.NAME}", key, value);
        }

        public static T GetConfig<T>(string key, T _default) {
            return Get(Registry.CurrentUser, $"Software\\{Reference.NAME}", key, _default);
        }

        public static string GetCurrentWallpapaer() {
            byte[] rawPath = (byte[])Get<object>(Registry.CurrentUser, "Control Panel\\Desktop", "TranscodedImageCache", null);
            byte[] path = new byte[rawPath.Length - 24];
            Array.Copy(rawPath, 24, path, 0, path.Length);

            return Encoding.Unicode.GetString(path).TrimEnd("\0".ToCharArray());
        }
    }
}
