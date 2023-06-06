using CommandLine;
using GLSLWallpaper.Preview;

Parser.Default.ParseArguments<Options>(args).WithParsed(options => {
    new Window(options.File, options.Watch).Run();
});

[Serializable]
class Options {
    [Value(0, MetaName = "file", HelpText = "Shader file")]
    public string File { get; set; } = null!;
    
    [Option('w', "watch", HelpText = "Watch file")]
    public bool Watch { get; set; }
}
