using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using IniParser;
using IniParser.Model;

namespace GLSLWallpapers.Helpers {
    public static class ShaderRegistry {
        public static Dictionary<string, ShaderInfo> Shaders { get; } = new Dictionary<string, ShaderInfo>();

        public static void Load() {
            if (!Directory.Exists(Reference.SHADERS_DIRECTORY)) {
                Directory.CreateDirectory(Reference.SHADERS_DIRECTORY);
            }

            foreach (string path in Directory.GetFiles(Reference.SHADERS_DIRECTORY, $"*.{Reference.SHADER_FILE_EXTENSION}")) {
                ShaderInfo info = ShaderInfo.FromFile(path);
                Shaders.Add(info.FileName, info);
            }
        }
    }

    public class ShaderInfo {
        public string FilePath { get; private set; }
        public string FileName => Path.GetFileNameWithoutExtension(FilePath);
        public string Name { get; private set; }
        public string Author { get; private set; }
        public string Code { get; private set; }
        public Image Image { get; private set; }

        public static ShaderInfo FromFile(string path) {
            ShaderInfo info = new ShaderInfo {
                FilePath = Path.GetFullPath(path)
            };

            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                ZipArchive file = new ZipArchive(stream, ZipArchiveMode.Read);

                foreach (ZipArchiveEntry entry in file.Entries) {
                    using (Stream entryStream = entry.Open()) {
                        if (entry.Name == "info.ini") {
                            using (StreamReader reader = new StreamReader(entryStream)) {
                                SectionData packInfo = new StreamIniDataParser().ReadData(reader).Sections.GetSectionData("Info");
                                if (packInfo != null) {
                                    info.Name = packInfo.Keys.ContainsKey("Name") ? packInfo.Keys.GetKeyData("Name").Value : "Unnamed";
                                    info.Author = packInfo.Keys.ContainsKey("Author") ? packInfo.Keys.GetKeyData("Author").Value : "Unknown";
                                }
                            }
                        } else if (entry.Name == "shader.glsl") {
                            using (StreamReader reader = new StreamReader(entryStream)) {
                                info.Code = reader.ReadToEnd();
                            }
                        } else if (entry.Name == "thumbnail.png") {
                            info.Image = Image.FromStream(entryStream);
                        }
                    }
                }
            }

            return info;
        }
    }
}