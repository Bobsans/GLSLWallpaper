using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GLSLWallpapers.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ButtonState = OpenTK.Input.ButtonState;

namespace GLSLWallpapers.Display {
    public class DisplayWindow : GameWindow {
        static readonly ConcurrentQueue<ShaderInfo> queue = new ConcurrentQueue<ShaderInfo>();
        float time;
        int vbo;

        ShaderProgram Program { get; set; }

        Attribute AttributePosition { get; } = new Attribute("position");
        Uniform UniformResolution { get; } = new Uniform("resolution");
        Uniform UniformTime { get; } = new Uniform("time");
        Uniform UniformMouse { get; } = new Uniform("mouse");

        public static DisplayWindow Instance { get; private set; }

        public DisplayWindow() : base(1, 1, GraphicsMode.Default, Reference.NAME, GameWindowFlags.FixedWindow) {
            Win32.SetWindowAsDesktopChild(WindowInfo.Handle);

            WindowBorder = WindowBorder.Hidden;
            Location = new Point(0, 0);
            Size = new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);

            Config.FramesPerSecondChange += (sender, i) => TargetRenderFrequency = i;
            Config.UpdatesPerSecondChange += (sender, i) => TargetUpdateFrequency = i;
            Config.ShaderChange += (sender, s) => queue.Enqueue(ShaderRegistry.Get(s));

            Instance = this;
        }

        protected override void OnLoad(EventArgs e) {
            GL.ClearColor(Color.Black);

            InitVbo();

            ShaderInfo shader = Config.ShaderName != null ? ShaderRegistry.Get(Config.ShaderName) : ShaderRegistry.First();
            SetShaders(shader?.VertexCode, shader?.FragmentCode);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e) {
            FreeVbo();
            Program?.Dispose();
            Win32.RefreshWallpaper();

            base.OnUnload(e);

            Logger.Debug("Display window unloaded");
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        void SetShaders(string vertex, string fragment) {
            Shader[] shaders = {
                new Shader(ShaderType.VertexShader, vertex ?? Shader.DEFAULT_VERTEX_SHADER),
                new Shader(ShaderType.FragmentShader, fragment ?? Shader.DEFAULT_FRAGMENT_SHADER)
            };

            CheckOpenGLerror();

            if (shaders.All(shader => shader.Compiled)) {
                Program?.Dispose();
                InitShader(shaders);
            } else {
                MessageBox.Show("Invalid sahder program", "Error");
            }
        }

        void InitShader(params Shader[] shaders) {
            Program = new ShaderProgram(shaders);
            CheckOpenGLerror();

            AttributePosition.GetLocation(Program);

            UniformTime.GetLocation(Program);
            UniformResolution.GetLocation(Program);
            UniformMouse.GetLocation(Program);

            Program.Use();
            CheckOpenGLerror();
        }

        void InitVbo() {
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.BufferData(BufferTarget.ArrayBuffer, 4 * Vector2.SizeInBytes, new[] {
                new Vector2(-1.0f, -1.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, -1.0f)
            }, BufferUsageHint.StaticDraw);
        }

        void FreeVbo() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(vbo);
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            queue.TryDequeue(out ShaderInfo info);
            if (info != null) {
                try {
                    SetShaders(info.VertexCode, info.FragmentCode);
                } catch (Exception ex) {
                    Logger.Error($"Could not apply shader program. Maybe it invalid. Exception: {ex}");
                }
            }

            UniformResolution.Set2F(ClientRectangle.Width, ClientRectangle.Height);

            time += (float)e.Time * ((float)Config.TimeScale / 1000);
            UniformTime.Set1F(time);

            if (Config.MouseInteract) {
                MouseState mouse = Mouse.GetState();
                UniformMouse.Set4F(mouse.X, mouse.Y, mouse.LeftButton == ButtonState.Pressed ? 1 : 0, mouse.RightButton == ButtonState.Pressed ? 1 : 0);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            GL.DisableVertexAttribArray(0);

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        static void CheckOpenGLerror() {
            ErrorCode code = GL.GetError();

            if (code != ErrorCode.NoError) {
                Logger.Error($"OpenGL error: {code}");
            }
        }
    }
}
