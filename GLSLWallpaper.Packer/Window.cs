using System.Buffers;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using GLSLWallpaper.Common;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace GLSLWallpaper.Packer;

public sealed class Window : ShaderWindow {
    static readonly ConcurrentQueue<string?> _queue = new();
    double _time;
    readonly Action<SKBitmap> _onCapture;

    public Window(string? shaderSource, Action<SKBitmap> onCapture) : base(new NativeWindowSettings {
        Location = new Vector2i(0, 0),
        Size = new Vector2i(1920, 1080),
        WindowBorder = WindowBorder.Hidden
    }) {
        _onCapture = onCapture;
        _queue.Enqueue(shaderSource);
    }

    protected override void OnUpdateFrame(FrameEventArgs e) {
        if (_queue.TryDequeue(out string? result)) {
            SetShader(result);
        }

        if (MouseState.IsButtonPressed(MouseButton.Left)) {
            _onCapture(GrabScreenshot());
            Close();
        }

        base.OnUpdateFrame(e);
    }

    protected override void SetUniforms(ShaderProgram program, double elapsed) {
        program.SetFloat("time", (float)(_time += elapsed));
        program.SetVector2("resolution", new Vector2(ClientRectangle.Size.X, ClientRectangle.Size.Y));
    }

    SKBitmap GrabScreenshot() {
        GCHandle handle = GCHandle.Alloc(ArrayPool<byte>.Shared.Rent(Size.X * Size.Y * 4), GCHandleType.Pinned);
        GL.ReadPixels(0, 0, Size.X, Size.Y, PixelFormat.Rgba, PixelType.UnsignedByte, handle.AddrOfPinnedObject());
        SKBitmap bitmap = new();
        SKImageInfo info = new(Size.X, Size.Y);
        bitmap.InstallPixels(info, handle.AddrOfPinnedObject(), info.RowBytes, (_, _) => handle.Free(), null);
        return bitmap;
    }
}
