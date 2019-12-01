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

namespace GLSLWallpapers.Display {
    public class DisplayWindow : GameWindow {
        ShaderProgram Program { get; set; }

        Uniform UniformTime { get; } = new Uniform("time");
        Uniform UniformResolution { get; } = new Uniform("resolution");
        Uniform UniformMouse { get; } = new Uniform("mouse");

//        int vbo;

        float Time { get; set; }

        public static readonly ConcurrentQueue<string> QUEUE = new ConcurrentQueue<string>();

        public static DisplayWindow Instance { get; private set; }

        public DisplayWindow() : base(1, 1, GraphicsMode.Default, Reference.NAME, GameWindowFlags.FixedWindow) {
            WindowBorder = WindowBorder.Hidden;
            Instance = this;

            Win32.SetWindowAsDesktopChild(WindowInfo.Handle);
            
            Size = new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            Location = new Point(0, 0);
            
            Config.UpdateFrequencyChange += (sender, i) => TargetRenderFrequency = i;
            Config.ShaderChange += (sender, s) => QUEUE.Enqueue(ShaderRegistry.Shaders[s].Code);
        }
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            GL.ClearColor(Color.Black);

//            InitVbo();

            SetShaders(null, ShaderRegistry.Shaders.ContainsKey(Config.ShaderName) ? ShaderRegistry.Shaders[Config.ShaderName].Code : null);
            Visible = true;
        }

        protected override void OnUnload(EventArgs e) {
//            FreeVbo();
            Program?.Dispose();

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
                MessageBox.Show($"Invalid sahder program", "Error");
            }
        }

        void InitShader(params Shader[] shaders) {
            Program = new ShaderProgram(shaders);
            CheckOpenGLerror();

            UniformTime.GetLocation(Program);
            UniformResolution.GetLocation(Program);
            UniformMouse.GetLocation(Program);

            Program.Use();
            CheckOpenGLerror();
        }

//        void InitVbo() {
//            vbo = GL.GenBuffer();
//
//            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
//
//            Vector2[] buffer = {new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1)};
//
//            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * buffer.Length, buffer, BufferUsageHint.StaticDraw);
//            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
//        }
//
//        void FreeVbo() {
//            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
//            GL.DeleteBuffer(vbo);
//        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            QUEUE.TryDequeue(out string code);
            if (code != null) {
                try {
                    SetShaders(null, code);
                } catch (Exception ex) {
                    Logger.Error($"Could not apply shader programm. Maybe it invalid. Exception: {ex}");
                }
            }

            UniformResolution.Set2F(ClientRectangle.Width, ClientRectangle.Height);

            Time += (float)e.Time * ((float)Config.TimeScale / 1000);
            UniformTime.Set1F(Time);

            if (Config.MouseInteract) {
                MouseState mouse = Mouse.GetState();
                UniformMouse.Set2F(mouse.X, mouse.Y);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(-1.0, -1.0);
            GL.Vertex2(1.0, -1.0);
            GL.Vertex2(1.0, 1.0);
            GL.Vertex2(-1.0, 1.0);
            GL.End();

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        public static void CheckOpenGLerror() {
            ErrorCode code = GL.GetError();

            if (code != ErrorCode.NoError) {
                Logger.Error($"OpenGL error: {code}");
            }
        }
    }
}