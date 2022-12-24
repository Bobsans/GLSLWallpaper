using DimTim.Configuration;

namespace GLSLWallpaper.Packer; 

public class Settings : IConfig {
    public string OutputPath { get; set; } = null!;
}
