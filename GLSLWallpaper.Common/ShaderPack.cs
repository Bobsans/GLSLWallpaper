using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using YamlDotNet.Serialization;

namespace GLSLWallpaper.Common;

public class ShaderPack {
    string? _hash;

    public bool Loaded { get; set; }

    public string FilePath { get; set; }
    public PackMeta Meta { get; set; } = new();
    public byte[]? Thumbnail { get; set; }
    public string? ShaderCode { get; set; }

    public ShaderPack(string path) {
        FilePath = path;
    }

    public string Hash => _hash ??= string.Concat(MD5
        .HashData(Encoding.UTF8.GetBytes($"{Meta.Name}:{Meta.Author}"))
        .Select(b => b.ToString("X2"))
    );

    public ShaderPack Load(bool force = false) {
        if (Loaded && !force) {
            return this;
        }

        using FileStream stream = File.OpenRead(FilePath);
        using ZipArchive zip = new(stream, ZipArchiveMode.Read);

        foreach (ZipArchiveEntry entry in zip.Entries) {
            using Stream entryStream = entry.Open();
            switch (entry.Name) {
                case "meta.yaml": {
                    using StreamReader reader = new(entryStream);
                    IDeserializer serializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
                    Meta = serializer.Deserialize<PackMeta>(reader);
                    break;
                }
                case "shader.frag": {
                    using StreamReader reader = new(entryStream);
                    ShaderCode = reader.ReadToEnd();
                    break;
                }
                case "thumbnail.png": {
                    using BinaryReader reader = new(entryStream);
                    Thumbnail = reader.ReadBytes((int)entry.Length);
                    break;
                }
            }
        }

        Loaded = true;
        return this;
    }

    [Serializable]
    public class PackMeta {
        public string Name { get; set; } = null!;
        public string Author { get; set; } = null!;
    }
}
