using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GLSLWallpaper.Common;

public abstract class ShaderWindow : GameWindow {
    protected ShaderProgram? ShaderProgram { get; set; }

    int _vbo;
    int _vao;

    readonly float[] _vertices = { -1.0f, -1.0f, 1.0f, -1.0f, -1.0f, 1.0f, 1.0f, -1.0f, 1.0f, 1.0f, -1.0f, 1.0f };

    protected ShaderWindow(NativeWindowSettings settings) : base(GameWindowSettings.Default, settings) {}

    protected override void OnLoad() {
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        base.OnLoad();
    }

    protected override void OnUnload() {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(_vbo);
        GL.BindVertexArray(0);
        GL.DeleteVertexArray(_vao);

        ShaderProgram?.Dispose();

        base.OnUnload();
    }

    protected override void OnResize(ResizeEventArgs e) {
        GL.Viewport(0, 0, e.Width, e.Height);

        base.OnResize(e);
    }

    protected override void OnUpdateFrame(FrameEventArgs e) {
        if (ShaderProgram != null) {
            SetUniforms(ShaderProgram, e.Time);
        }

        base.OnUpdateFrame(e);
    }

    protected override void OnRenderFrame(FrameEventArgs e) {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        
        SwapBuffers();

        base.OnRenderFrame(e);
    }

    protected virtual void SetShader(string? shaderSource) {
        ShaderProgram = new ShaderProgram(shaderSource);
        ShaderProgram.Use();
    }

    protected abstract void SetUniforms(ShaderProgram program, double elapsed);
}
