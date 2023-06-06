using CommandLine;

namespace GLSLWallpaper.Packer; 

[Verb("pack", HelpText = "Pack shaders"), Serializable]
public class PackOptions {
    [Option("input", HelpText = "Shaders sources path")]
    public string InputPath { get; set; } = null!;

    [Option("output", HelpText = "Output path")]
    public string OutputPath { get; set; } = null!;
}
