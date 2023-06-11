using CommandLine;
using GLSLWallpaper.Packer;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
Config config = deserializer.Deserialize<Config>(File.ReadAllText(Path.Join(Config.Root, "config.yaml")));

Parser.Default.ParseArguments<AddOptions, PackOptions>(args).MapResult(
    (AddOptions opts) => Worker.Add(opts, config),
    (PackOptions opts) => Worker.Pack(opts, config),
    _ => 1
);
