using DimTim.Configuration;
using DimTim.Utils.Logging;

namespace GLSLWallpaper.Site;

[Serializable]
public readonly record struct Config() : IConfig {
    public static string Root => Path.GetDirectoryName(typeof(Config).Assembly.Location) ?? throw new Exception("Invalid assembly path!");

    public string Host { get; init; } = "+";
    public int Port { get; init; } = 8000;

    public LogLevel LogLevel { get; init; } = LogLevel.Debug;

    public string TemplatePath { get; init; } = "web/template";
};
