using System.Drawing.Imaging;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using GLSLWallpaper.Common;
using SkiaSharp;
using YamlDotNet.Serialization;

namespace GLSLWallpaper.Packer;

public static class Worker {
    static readonly Regex _nameRegex = new(@"/\*\s+Name:\s+(?<value>.*)\s+\*/", RegexOptions.Compiled);
    static readonly Regex _authorRegex = new(@"/\*\s+Author:\s+(?<value>.*)\s+\*/", RegexOptions.Compiled);

    public static int Pack(PackOptions options, Settings settings) {
        if (string.IsNullOrEmpty(options.OutputPath)) {
            Console.Error.WriteLine("Output path not specified!");
            return 1;
        }

        foreach (string directory in Directory.EnumerateDirectories(options.InputPath)) {
            if (!Directory.Exists(options.OutputPath)) {
                Directory.CreateDirectory(options.OutputPath);
            }

            string shaderPath = Path.Join(directory, "shader.frag");
            string metaPath = Path.Join(directory, "meta.yaml");
            string previewPath = Path.Join(directory, "preview.png");

            if (!File.Exists(shaderPath)) {
                Console.WriteLine("No shader found!");
                return 1;
            }

            if (!File.Exists(metaPath)) {
                Console.WriteLine("No meta file found!");
                return 1;
            }

            if (!File.Exists(previewPath)) {
                Console.WriteLine("No preview found!");
                return 1;
            }

            string outFilePath;

            using (Stream metaStream = File.OpenRead(metaPath)) {
                using StreamReader reader = new(metaStream);
                ShaderPack.PackMeta meta = new DeserializerBuilder().Build().Deserialize<ShaderPack.PackMeta>(reader);
                outFilePath = Path.Join(options.OutputPath, $"{GetHash(meta.Name, meta.Author)}.{Constants.PACK_EXTENSION}");
            }

            Console.WriteLine($"Pack \"{Path.GetFileName(directory)}\" -> \"{Path.GetFileName(outFilePath)}\"...");

            using FileStream stream = File.Create(outFilePath);
            using ZipArchive zip = new(stream, ZipArchiveMode.Create);

            if (File.Exists(shaderPath)) {
                using Stream es = zip.CreateEntry("shader.frag").Open();
                es.Write(File.ReadAllBytes(shaderPath));
            }

            using (Stream es = zip.CreateEntry("meta.yaml").Open()) {
                es.Write(File.ReadAllBytes(metaPath));
            }

            using (Stream es = zip.CreateEntry("thumbnail.png").Open()) {
                SKBitmap
                    .Decode(File.ReadAllBytes(previewPath))
                    .Resize(new SKSizeI(128, 72), SKFilterQuality.Medium)
                    .Encode(es, SKEncodedImageFormat.Png, 50);
            }
        }

        return 0;
    }

    public static int Add(AddOptions options, Settings settings) {
        if (string.IsNullOrEmpty(options.ShaderPath)) {
            Console.Error.WriteLine("Invalid shader file path!");
            return -1;
        }

        if (!File.Exists(options.ShaderPath)) {
            Console.Error.WriteLine($"File {options.ShaderPath} not found!");
            return -1;
        }

        string outPath = Path.Join(string.IsNullOrEmpty(options.OutputPath) ? settings.OutputPath : options.OutputPath, ToUrlSlug(options.Name));

        if (Directory.Exists(outPath)) {
            Console.Write($"Directory {outPath} already exists! Rewrite? (y/n): ");
            string? res = Console.ReadLine();
            if (res?.ToLower() == "y") {
                foreach (string entry in Directory.EnumerateFiles(outPath)) {
                    File.Delete(entry);
                }
            } else {
                return 1;
            }
        }

        string shader = File.ReadAllText(options.ShaderPath);

        new Window(shader, bmp => {
                using MemoryStream s = new();
                bmp.Save(s, ImageFormat.Png);
                byte[] buff = s.GetBuffer();
                int end = buff.Length - 1;
                while (buff[end--] == 0) {}

                Array.Resize(ref buff, end);

                if (!Directory.Exists(outPath)) {
                    Directory.CreateDirectory(outPath);
                }

                using Stream ens = File.Create(Path.Join(outPath, "preview.png"));
                SKBitmap.Decode(buff).Encode(ens, SKEncodedImageFormat.Png, 50);
            }
        ).Run();

        string name = options.Name;
        string author = options.Author;

        if (string.IsNullOrEmpty(name)) {
            Match nameMatch = _nameRegex.Match(shader);
            if (nameMatch.Success) {
                name = nameMatch.Groups["value"].Value;
                _nameRegex.Replace(shader, string.Empty);
            } else {
                Console.Error.WriteLine("Name not specified!");
                return -1;
            }
        }

        if (string.IsNullOrEmpty(author)) {
            Match authorMatch = _authorRegex.Match(shader);
            if (authorMatch.Success) {
                author = authorMatch.Groups["value"].Value;
                _authorRegex.Replace(shader, string.Empty);
            } else {
                Console.Error.WriteLine("Author not specified!");
                return -1;
            }
        }

        File.WriteAllText(Path.Join(outPath, "shader.frag"), shader.Trim());

        using Stream ems = File.Create(Path.Join(outPath, "meta.yaml"));
        using TextWriter tw = new StreamWriter(ems);
        new SerializerBuilder().Build().Serialize(tw, new ShaderPack.PackMeta { Name = name, Author = author });

        return 0;
    }

    static string ToUrlSlug(string value) {
        value = value.ToLowerInvariant();
        value = value.Normalize(NormalizationForm.FormKD);
        value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);
        value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);
        value = value.Trim('-', '_');
        value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

        return value;
    }

    static string GetHash(string name, string author) {
        return string.Concat(MD5
            .HashData(Encoding.UTF8.GetBytes($"{name}:{author}"))
            .Select(b => b.ToString("X2"))
        );
    }
}
