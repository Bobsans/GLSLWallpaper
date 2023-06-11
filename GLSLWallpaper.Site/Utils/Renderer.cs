using Stubble.Core;
using Stubble.Core.Builders;
using Stubble.Core.Interfaces;
using Stubble.Core.Loaders;

namespace GLSLWallpaper.Site.Utils;

public class Renderer {
    readonly StubbleVisitorRenderer _stubble;

    public Renderer(string templatePath) {
        _stubble ??= new StubbleBuilder().Configure(builder => builder
            .SetTemplateLoader(new CompositeLoader(new FileLoader(Path.GetFullPath(templatePath), "mustache", "html")))
        ).Build();
    }

    public string Render(string name, object? context = null) => _stubble.Render(name, context);
    public ValueTask<string> RenderAsync(string name, object? context = null) => _stubble.RenderAsync(name, context);

    class FileLoader : IStubbleLoader {
        readonly string _path;
        readonly string[] _extensions;

        public FileLoader(string path, params string[] extensions) {
            _path = path;
            _extensions = extensions;
        }

        public string? Load(string name) {
            foreach (string extension in _extensions) {
                string filePath = Path.Join(_path, $"{name}.{extension}");
                if (File.Exists(filePath)) {
                    return File.ReadAllText(filePath);
                }
            }

            return null;
        }

        public async ValueTask<string?> LoadAsync(string name) {
            foreach (string extension in _extensions) {
                string filePath = Path.Join(_path, $"{name}.{extension}");
                if (File.Exists(filePath)) {
                    return await File.ReadAllTextAsync(filePath);
                }
            }

            return null;
        }

        public IStubbleLoader Clone() => new FileLoader(_path, _extensions);
    }
}
