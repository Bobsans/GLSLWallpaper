using System;
using GLSLWallpapers.Helpers;
using SciterSharp;

namespace GLSLWallpapers {
    public static class Config {
        static double timeScale = 1; /* 0.01..2 */
        static int framesPerSecond = 120; /* 1..120 */
        static int updatesPerSecond = 120; /* 1..120 */
        static bool mouseInteract = true;
        static string shaderName;

        public static double TimeScale {
            get => timeScale;
            set {
                timeScale = MathUtils.Clamp(value, 0.01, 2);
                TimeScaleChange?.Invoke(null, timeScale);
            }
        }

        public static int FramesPerSecond {
            get => framesPerSecond;
            set {
                framesPerSecond = MathUtils.Clamp(value, 1, 120);
                FramesPerSecondChange?.Invoke(null, framesPerSecond);
            }
        }

        public static int UpdatesPerSecond {
            get => updatesPerSecond;
            set {
                updatesPerSecond = MathUtils.Clamp(value, 1, 120);
                UpdatesPerSecondChange?.Invoke(null, updatesPerSecond);
            }
        }

        public static bool MouseInteract {
            get => mouseInteract;
            set {
                mouseInteract = value;
                MouseInteractChange?.Invoke(null, value);
            }
        }

        public static string ShaderName {
            get => shaderName;
            set {
                shaderName = value;
                ShaderChange?.Invoke(null, value);
            }
        }

        public static SciterValue SciterValue =>
            new SciterValue {
                ["FramesPerSecond"] = new SciterValue(FramesPerSecond),
                ["UpdatesPerSecond"] = new SciterValue(UpdatesPerSecond),
                ["TimeScale"] = new SciterValue(TimeScale),
                ["MouseInteract"] = new SciterValue(MouseInteract),
                ["ShaderName"] = new SciterValue(ShaderName)
            };

        public static event EventHandler<double> TimeScaleChange;
        public static event EventHandler<int> FramesPerSecondChange;
        public static event EventHandler<int> UpdatesPerSecondChange;
        public static event EventHandler<bool> MouseInteractChange;
        public static event EventHandler<string> ShaderChange;

        public static void Load() {
            TimeScale = RegistryUtils.GetConfig("TimeScale", TimeScale);
            FramesPerSecond = RegistryUtils.GetConfig("FramesPerSecond", FramesPerSecond);
            UpdatesPerSecond = RegistryUtils.GetConfig("UpdatesPerSecond", UpdatesPerSecond);
            MouseInteract = RegistryUtils.GetConfig("MouseInteract", MouseInteract);
            ShaderName = RegistryUtils.GetConfig("ShaderName", ShaderName);
        }

        public static void Save() {
            RegistryUtils.SetConfig("TimeScale", TimeScale);
            RegistryUtils.SetConfig("FramesPerSecond", FramesPerSecond);
            RegistryUtils.SetConfig("UpdatesPerSecond", UpdatesPerSecond);
            RegistryUtils.SetConfig("MouseInteract", MouseInteract);
            RegistryUtils.SetConfig("ShaderName", ShaderName);
        }

        public static void SetFieldValue<T>(string name, T value) {
            typeof(Config).GetProperty(name)?.SetValue(null, value);
        }

        public static T GetFieldValue<T>(string name) {
            return (T)typeof(Config).GetProperty(name)?.GetValue(null);
        }
    }
}
