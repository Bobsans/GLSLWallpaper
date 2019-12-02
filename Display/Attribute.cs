using GLSLWallpapers.Helpers;

namespace GLSLWallpapers.Display {
    public class Attribute {
        public int Handle { get; private set; } = -1;
        public string Name { get; }

        public Attribute(string name) {
            Name = name;
        }

        public void GetLocation(ShaderProgram program) {
            Handle = program.GetAttributeLocation(Name);

            if (Handle == -1) {
                Logger.Warning($"Could not bind attribute {Name}");
            }
        }
    }
}
