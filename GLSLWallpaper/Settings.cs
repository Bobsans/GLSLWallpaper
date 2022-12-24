using System;
using System.IO;
using System.Reflection;

namespace GLSLWallpaper;

public static class Settings {
    public static readonly string ROOT = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Identity))!.Location)!;
    public static readonly string PACKS_DIRECTORY = Path.Combine(ROOT, "Packs");

    static double _timeScale = 1.0; // 0.0..2.0
    static int _framesPerSecond = 60; // 0..120
    static int _updatesPerSecond = 60; // 0..120
    static bool _mouseInteract = true;
    static string? _currentShader;

    public static double TimeScale {
        get => _timeScale;
        set => TimeScaleChanged.Invoke(_timeScale = Math.Clamp(value, 0.0, 2.0));
    }

    public static int FramesPerSecond {
        get => _framesPerSecond;
        set => FramesPerSecondChanged.Invoke(_framesPerSecond = Math.Clamp(value, 0, 120));
    }

    public static int UpdatesPerSecond {
        get => _updatesPerSecond;
        set => UpdatesPerSecondChanged.Invoke(_updatesPerSecond = Math.Clamp(value, 0, 120));
    }

    public static bool MouseInteract {
        get => _mouseInteract;
        set => MouseInteractChanged.Invoke(_mouseInteract = value);
    }

    public static string? CurrentShader {
        get => _currentShader;
        set => CurrentShaderChanged.Invoke(_currentShader = value!);
    }

    public static event Action<double> TimeScaleChanged = delegate {};
    public static event Action<int> FramesPerSecondChanged = delegate {};
    public static event Action<int> UpdatesPerSecondChanged = delegate {};
    public static event Action<bool> MouseInteractChanged = delegate {};
    public static event Action<string> CurrentShaderChanged = delegate {};

    public static void Load() {
        TimeScale = RegistryHelper.GetConfig(nameof(TimeScale), TimeScale);
        FramesPerSecond = RegistryHelper.GetConfig(nameof(FramesPerSecond), FramesPerSecond);
        UpdatesPerSecond = RegistryHelper.GetConfig(nameof(UpdatesPerSecond), UpdatesPerSecond);
        MouseInteract = RegistryHelper.GetConfig(nameof(MouseInteract), MouseInteract);
        CurrentShader = RegistryHelper.GetConfig(nameof(CurrentShader), CurrentShader);
    }

    public static void Save() {
        RegistryHelper.SetConfig(nameof(TimeScale), TimeScale);
        RegistryHelper.SetConfig(nameof(FramesPerSecond), FramesPerSecond);
        RegistryHelper.SetConfig(nameof(UpdatesPerSecond), UpdatesPerSecond);
        RegistryHelper.SetConfig(nameof(MouseInteract), MouseInteract);

        if (CurrentShader != null) {
            RegistryHelper.SetConfig(nameof(CurrentShader), CurrentShader);
        }
    }
}
