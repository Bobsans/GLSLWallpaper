using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GLSLWallpaper.Common;

public class ShaderProgram {
    const string DEFAULT_FRAGMENT_SHADER = "void main() { gl_fragColor = vec4(1.0, 1.0, 1.0, 1.0); }";

    public int Handle { get; }
    public Dictionary<string, int> Uniforms { get; }

    public ShaderProgram(string? shaderSource) {
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, !string.IsNullOrEmpty(shaderSource) ? shaderSource : DEFAULT_FRAGMENT_SHADER);
        CompileShader(fragmentShader);

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, fragmentShader);

        LinkProgram(Handle);

        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(fragmentShader);

        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
        Uniforms = new Dictionary<string, int>();
        for (int i = 0; i < numberOfUniforms; i++) {
            string? key = GL.GetActiveUniform(Handle, i, out _, out _);
            int location = GL.GetUniformLocation(Handle, key);
            Uniforms.Add(key, location);
        }
    }

    public void Dispose() {
        GL.UseProgram(0);
        GL.DeleteProgram(Handle);
    }
    
    public void Use() {
        GL.UseProgram(Handle);
    }

    static void CompileShader(int shader) {
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);
        if (code != (int)All.True) {
            throw new Exception($"Error occurred whilst compiling Shader({shader})\n{GL.GetShaderInfoLog(shader)}");
        }
    }

    static void LinkProgram(int program) {
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int code);
        if (code != (int)All.True) {
            throw new Exception($"Error occurred whilst linking Program({program})");
        }
    }

    public bool HasUniform(string name) {
        return Uniforms.ContainsKey(name);
    }

    public void SetFloat(string name, float value) {
        if (Uniforms.TryGetValue(name, out int location)) {
            GL.Uniform1(location, value);
        }
    }

    public void SetVector2(string name, Vector2 value) {
        if (Uniforms.TryGetValue(name, out int location)) {
            GL.Uniform2(location, value);
        }
    }

    public void SetVector3(string name, Vector3 value) {
        if (Uniforms.TryGetValue(name, out int location)) {
            GL.Uniform3(location, value);
        }
    }

    public void SetVector4(string name, Vector4 value) {
        if (Uniforms.TryGetValue(name, out int location)) {
            GL.Uniform4(location, value);
        }
    }
}
