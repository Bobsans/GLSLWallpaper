using CommandLine;

[Verb("add", HelpText = "Add shader to sources")]
public class AddOptions {
    [Option("shader", HelpText = "Fragment shader path")]
    public string ShaderPath { get; set; } = null!;
    
    [Option("author", HelpText = "Shader author")]
    public string Author { get; set; } = null!;

    [Option("name", HelpText = "Shader name")]
    public string Name { get; set; } = null!;

    [Option("output", HelpText = "Output path")]
    public string OutputPath { get; set; } = null!;
}
