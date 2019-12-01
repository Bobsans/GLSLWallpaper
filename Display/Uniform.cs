using GLSLWallpapers.Helpers;
using OpenTK.Graphics.OpenGL;

namespace GLSLWallpapers.Display {
    public class Uniform {
        public int Handle { get; private set; } = -1;
        public string Name { get; }

        public Uniform(string name) {
            Name = name;
        }

        public void GetLocation(ShaderProgram program) {
            Handle = program.GetUniformLocation(Name);

            if (Handle == -1) {
                Logger.Warning($"Could not bind uniform {Name}");
            }
        }

        public void Set1F(float x) {
            GL.Uniform1(Handle, x);
        }

        public void Set2F(float x, float y) {
            GL.Uniform2(Handle, x, y);
        }
        
        public void Set3F(float x, float y, float z) {
            GL.Uniform3(Handle, x, y, z);
        }

        public void Set4F(float x, float y, float z, float w) {
            GL.Uniform4(Handle, x, y, z, w);
        }
    }
}