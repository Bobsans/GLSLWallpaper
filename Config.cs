using System;
using GLSLWallpapers.Helpers;

namespace GLSLWallpapers {
    public static class Config {
        public static event EventHandler<int> TimeScaleChange;
        public static event EventHandler<int> FramesPerSecondChange;
        public static event EventHandler<int> UpdatesPerSecondChange;
        public static event EventHandler<bool> MouseInteractChange;
        public static event EventHandler<string> ShaderChange;

        static int _timeScale = 1000; /* 1..2000 */
        static int _framesPerSecond = 60; /* 1..120 */
        static int _updatesPerSecond = 60; /* 1..120 */
        static bool _mouseInteract;
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

        public static void Load() {
            TimeScale = Convert.ToInt32(RegistryUtils.Get("TimeScale", TimeScale));
            FramesPerSecond = Convert.ToInt32(RegistryUtils.Get("FramesPerSecond", FramesPerSecond));
            UpdatesPerSecond = Convert.ToInt32(RegistryUtils.Get("UpdatesPerSecond", UpdatesPerSecond));
            MouseInteract = Convert.ToBoolean(RegistryUtils.Get("MouseInteract", MouseInteract));
            ShaderName = Convert.ToString(RegistryUtils.Get("ShaderName", ShaderName));
        }

        public static void Save() {
            RegistryUtils.Save("TimeScale", TimeScale);
            RegistryUtils.Save("FramesPerSecond", FramesPerSecond);
            RegistryUtils.Save("UpdatesPerSecond", UpdatesPerSecond);
            RegistryUtils.Save("MouseInteract", MouseInteract);
            RegistryUtils.Save("ShaderName", ShaderName);
        }
    }
}