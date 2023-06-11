namespace GLSLWallpaper.Packer;

[Serializable]
public class Config {
    public static string Root { get; } = Path.GetDirectoryName(typeof(Config).Assembly.Location)!;

    public string OutputPath { get; set; } = null!;
}
