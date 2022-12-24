using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Windows;
using GLSLWallpaper.Common;
using Microsoft.Win32;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GLSLWallpaper;

public class BackgroundWindow : ShaderWindow {
    static BackgroundWindow? _instance;
    static readonly ConcurrentQueue<ShaderPack> _queue = new();
    double _time;

    BackgroundWindow() : base(new NativeWindowSettings {
        WindowBorder = WindowBorder.Hidden,
        Location = new Vector2i(0, 0),
        Size = new Vector2i((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight)
    }) {
        Settings.FramesPerSecondChanged += value => RenderFrequency = value;
        Settings.UpdatesPerSecondChanged += value => UpdateFrequency = value;
        Settings.CurrentShaderChanged += value => EnqueueShader(PackRegistry.GetByHash(value, () => PackRegistry.Packs.First()));

        SystemEvents.DisplaySettingsChanged += (_, _) => Size = new Vector2i(
            (int)SystemParameters.VirtualScreenWidth,
            (int)SystemParameters.VirtualScreenHeight
        );

        _instance = this;
    }

    public static BackgroundWindow Get() {
        return _instance ??= new BackgroundWindow();
    }

    public static void RunThreaded() {
        Get().Context.MakeNoneCurrent();
        new Thread(() => Get().Run()) { IsBackground = true }.Start();
    }

    public static void EnqueueShader(ShaderPack pack) {
        _queue.Enqueue(pack);
    }

    protected override void OnUpdateFrame(FrameEventArgs e) {
        if (_queue.TryDequeue(out ShaderPack? data)) {
            SetShader(data.ShaderCode);
        }

        base.OnUpdateFrame(e);
    }

    protected override void SetShader(string? shaderSource) {
        try {
            base.SetShader(shaderSource);
        } catch (Exception ex) {
            MessageBox.Show($@"Invalid shader program: {ex}", @"Error");
        }
    }

    protected override void SetUniforms(ShaderProgram program, double elapsed) {
        program.SetFloat("time", (float)(_time += elapsed * Settings.TimeScale));
        program.SetVector2("resolution", new Vector2(ClientRectangle.Size.X, ClientRectangle.Size.Y));

        if (Settings.MouseInteract && program.HasUniform("mouse")) {
            Win32.MouseState mouse = Win32.GetMouseState();
            program.SetVector4("mouse", new Vector4(mouse.Position.X, mouse.Position.Y, mouse.Left ? 1 : 0, mouse.Right ? 1 : 0));
        }
    }
}
