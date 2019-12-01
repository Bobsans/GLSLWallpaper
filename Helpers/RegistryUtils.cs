using System;
using System.Linq;
using Microsoft.Win32;

namespace GLSLWallpapers.Helpers {
    public static class RegistryUtils {
        public static void Save(string key, object value) {
            using (RegistryKey software = Registry.CurrentUser.OpenSubKey("Software", true)) {
                if (software != null) {
                    if (!software.GetSubKeyNames().Contains(Reference.NAME)) {
                        software.CreateSubKey(Reference.NAME);
                    }

                    using (RegistryKey app = software.OpenSubKey(Reference.NAME, true)) {
                        app?.SetValue(key, value);
                    }
                }
            }
        }

        public static object Get<T>(string key, T _default) {
            using (RegistryKey software = Registry.CurrentUser.OpenSubKey("Software")) {
                if (software != null) {
                    if (!software.GetSubKeyNames().Contains(Reference.NAME)) {
                        return _default;
                    }

                    using (RegistryKey app = software.OpenSubKey(Reference.NAME)) {
                        return app?.GetValue(key);
                    }
                }
            }

            return _default;
        }
    }
}