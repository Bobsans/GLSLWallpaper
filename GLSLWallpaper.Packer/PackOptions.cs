using CommandLine;

[Verb("pack", HelpText = "Pack shaders")]
public class PackOptions {
    [Option("input", HelpText = "Shaders sources path")]
    public string InputPath { get; set; } = null!;

    [Option("output", HelpText = "Output path")]
    public string OutputPath { get; set; } = null!;
}
