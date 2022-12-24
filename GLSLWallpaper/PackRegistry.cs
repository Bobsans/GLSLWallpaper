using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using GLSLWallpaper.Common;

namespace GLSLWallpaper;

public static class PackRegistry {
    public static List<ShaderPack> Packs { get; } = new();

    public static void Load() {
        if (!Directory.Exists(Settings.PACKS_DIRECTORY)) {
            Directory.CreateDirectory(Settings.PACKS_DIRECTORY);
        }

        Packs.Clear();
        foreach (string path in Directory.GetFiles(Settings.PACKS_DIRECTORY, $"*.{Constants.PACK_EXTENSION}")) {
            Packs.Add(new ShaderPack(path).Load());
        }
    }

    public static ShaderPack GetByHash(string? hash, Func<ShaderPack> orElse) {
        return Packs.FirstOrDefault(it => it.Hash == hash) ?? orElse();
    }

    public static bool IsShaderPackFile(string path) {
        return File.Exists(path) && Path.GetExtension(path) == $".{Constants.PACK_EXTENSION}";
    }

    public static void Install(string path) {
        if (File.Exists(path)) {
            try {
                ShaderPack pack = new ShaderPack(path).Load();

                if (Packs.Any(it => it.Hash == pack.Hash)) {
                    MessageBoxResult result = MessageBox.Show(
                        $@"Wallpaper ""{pack.Meta.Name}"" already exist. Rewrite?",
                        @"Wallpaper exist",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning
                    );

                    if (result == MessageBoxResult.No) {
                        return;
                    }
                }

                string destPath = Path.Combine(Settings.PACKS_DIRECTORY, $"{pack.Hash}.{Constants.PACK_EXTENSION}");
                File.Copy(path, destPath, true);

                if (!PipeWorker.Send(new PipeWorker.Message { Action = PipeWorker.WALLPAPER_CHANGED, Value = pack.Hash })) {
                    pack.FilePath = destPath;
                    Packs.Add(pack);
                }
                
                RegistryHelper.SetConfig(nameof(Settings.CurrentShader), pack.Hash);

                return;
            } catch (Exception ex) {
                Logger.Error($"Error wallpaper installation: {ex}");
            }
        }

        MessageBox.Show($@"Invalid wallpaper file: ""{path}""");
    }
}
