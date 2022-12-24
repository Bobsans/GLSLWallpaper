using System.Reflection;
using CommandLine;
using DimTim.Configuration.Yaml;
using GLSLWallpaper.Packer;

Settings settings = YamlConfigProvider.Load<Settings>(Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "settings.yaml"));

Parser.Default.ParseArguments<AddOptions, PackOptions>(args).MapResult(
    (AddOptions opts) => Worker.Add(opts, settings),
    (PackOptions opts) => Worker.Pack(opts, settings),
    _ => 1
);
