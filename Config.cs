using System;
using GLSLWallpapers.Helpers;

namespace GLSLWallpapers {
    public static class Config {
        static int _timeScale = 1000; /* 1..2000 */
        static int _framesPerSecond = 120; /* 1..120 */
        static int _updatesPerSecond = 120; /* 1..120 */
        static bool _mouseInteract = true;
        static string _shaderName;

        public static int TimeScale {
            get => _timeScale;
            set {
                _timeScale = MathUtils.Clamp(value, 1, 2000);
                TimeScaleChange?.Invoke(null, _timeScale);
            }
        }

        public static int FramesPerSecond {
            get => _framesPerSecond;
            set {
                _framesPerSecond = MathUtils.Clamp(value, 1, 120);
                FramesPerSecondChange?.Invoke(null, _framesPerSecond);
            }
        }

        public static int UpdatesPerSecond {
            get => _updatesPerSecond;
            set {
                _updatesPerSecond = MathUtils.Clamp(value, 1, 120);
                UpdatesPerSecondChange?.Invoke(null, _updatesPerSecond);
            }
        }

        public static bool MouseInteract {
            get => _mouseInteract;
            set {
                _mouseInteract = value;
                MouseInteractChange?.Invoke(null, value);
            }
        }

        public static string ShaderName {
            get => _shaderName;
            set {
                _shaderName = value;
                ShaderChange?.Invoke(null, value);
            }
        }

        public static event EventHandler<int> TimeScaleChange;
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
    }
}
