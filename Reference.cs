using System.IO;

namespace GLSLWallpapers {
    public static class Reference {
        public const string NAME = "GLSLWallpaper";
        
        public const int VERSION_MAJOR = 0;
        public const int VERSION_MINOR = 1;
        
        public static readonly string VERSION = $"{VERSION_MAJOR}.{VERSION_MINOR}";

        public static readonly string DATA_DIRECTORY = Path.Combine(Directory.GetCurrentDirectory(), "data");
        public static readonly string SHADERS_DIRECTORY = Path.Combine(DATA_DIRECTORY, "shaders");
        
        public const string SHADER_FILE_EXTENSION = "glslwallpaper";
    }
}