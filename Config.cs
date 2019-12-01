using System;
using GLSLWallpapers.Helpers;

namespace GLSLWallpapers {
    public static class Config {
        public static event EventHandler<int> TimeScaleChange;
        public static event EventHandler<int> UpdateFrequencyChange;
        public static event EventHandler<bool> MouseInteractChange;
        public static event EventHandler<string> ShaderChange;

        static int time_scale = 1000;
        static int update_frequency = 60;
        static bool mouse_interact;
        static string shader_name;

        public static int TimeScale {
            get => time_scale;
            set {
                time_scale = value;
                TimeScaleChange?.Invoke(null, value);
            }
        }

        public static int UpdateFrequency {
            get => update_frequency;
            set {
                update_frequency = value;
                UpdateFrequencyChange?.Invoke(null, value);
            }
        }

        public static bool MouseInteract {
            get => mouse_interact;
            set {
                mouse_interact = value;
                MouseInteractChange?.Invoke(null, value);
            }
        }

        public static string ShaderName {
            get => shader_name;
            set {
                shader_name = value;
                ShaderChange?.Invoke(null, value);
            }
        }

        public static void Load() {
            TimeScale = Convert.ToInt32(RegistryUtils.Get("TimeScale", TimeScale));
            UpdateFrequency = Convert.ToInt32(RegistryUtils.Get("UpdateFrequency", UpdateFrequency));
            MouseInteract = Convert.ToBoolean(RegistryUtils.Get("MouseInteract", MouseInteract));
            ShaderName = Convert.ToString(RegistryUtils.Get("ShaderName", ShaderName));
        }

        public static void Save() {
            RegistryUtils.Save("TimeScale", TimeScale);
            RegistryUtils.Save("UpdateFrequency", UpdateFrequency);
            RegistryUtils.Save("MouseInteract", MouseInteract);
            RegistryUtils.Save("ShaderName", ShaderName);
        }
    }
}