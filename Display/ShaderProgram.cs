using System;
using OpenTK.Graphics.OpenGL;

namespace GLSLWallpapers.Display {
    public class ShaderProgram {
        public int Handle { get; }

        public ShaderProgram(params Shader[] shaders) {
            Handle = GL.CreateProgram();

            foreach (Shader shader in shaders) {
                GL.AttachShader(Handle, shader.Handle);
            }

            GL.LinkProgram(Handle);
            
            foreach (Shader shader in shaders) {
                GL.DetachShader(Handle, shader.Handle);
                shader.Delete();
            }
            
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int status);
            if (status == 0) {
                throw new Exception($"Could not attach shader program {GL.GetProgramInfoLog(Handle)}");
            }
        }

        public void Dispose() {
            GL.UseProgram(0);
            GL.DeleteProgram(Handle);
        }

        public int GetAttributeLocation(string name) {
            return GL.GetAttribLocation(Handle, name);
        }

        public int GetUniformLocation(string name) {
            return GL.GetUniformLocation(Handle, name);
        }

        public void Use() {
            GL.UseProgram(Handle);
        }
    }
}