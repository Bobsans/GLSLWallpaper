using GLSLWallpapers.Helpers;
using OpenTK.Graphics.OpenGL;

namespace GLSLWallpapers.Display {
    public class Shader {
        public const string DEFAULT_VERTEX_SHADER = "#version 330 core\nlayout (location = 0) in vec3 position;\nvoid main() {\ngl_Position = vec4(position, 1.0);\n}";
        public const string DEFAULT_FRAGMENT_SHADER = "#version 330 core\nout vec4 fragColor;\nvoid main() {\nfragColor = vec4(1.0, 1.0, 1.0, 1.0);\n}";

        public int Handle { get; }
        public bool Compiled { get; }

        public Shader(ShaderType type, string code) {
            Handle = GL.CreateShader(type);

            GL.ShaderSource(Handle, code);
            GL.CompileShader(Handle);

            GL.GetShader(Handle, ShaderParameter.CompileStatus, out int status);
            Compiled = status == 1;

            if (!Compiled) {
                Logger.Error($"Error compiling {type.ToString()} shader: {GL.GetShaderInfoLog(Handle)}");
            }
        }

        public void Delete() {
            GL.DeleteShader(Handle);
        }
    }
}
