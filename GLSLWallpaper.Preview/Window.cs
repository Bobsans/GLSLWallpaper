using System.Collections.Concurrent;
using GLSLWallpaper.Common;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace GLSLWallpaper.Preview;

public sealed class Window : ShaderWindow {
    static readonly ConcurrentQueue<string?> _queue = new();
    readonly bool _watch;
    double _time;
    string? _currentFile;
    FileSystemWatcher? _watcher;
    Task? _watchTask;
    readonly ErrorForm _errorForm;

    public Window(string? file, bool watch = false) : base(new NativeWindowSettings {
        Size = new Vector2i(960, 540)
    }) {
        _watch = watch;

        if (!string.IsNullOrEmpty(file)) {
            LoadShaderFromFile(file);
        }

        _errorForm = new ErrorForm(this);
    }

    protected override void OnUpdateFrame(FrameEventArgs e) {
        if (_queue.TryDequeue(out string? result)) {
            try {
                SetShader(result);
                _errorForm.Hide();
            } catch (Exception ex) {
                _errorForm.SetMessage(ex.Message);
                _errorForm.Show();
            }
        }

        base.OnUpdateFrame(e);
    }

    protected override void SetUniforms(ShaderProgram program, double elapsed) {
        program.SetFloat("time", (float)(_time += elapsed));
        program.SetVector2("resolution", new Vector2(ClientRectangle.Size.X, ClientRectangle.Size.Y));
    }

    void LoadShaderFromFile(string fileName) {
        if (File.Exists(fileName)) {
            _currentFile = Path.GetFullPath(fileName);
            _queue.Enqueue(File.ReadAllText(_currentFile));
            WatchForChangesIfNeed();
        }
    }

    void WatchForChangesIfNeed() {
        if (_watch && _currentFile != null) {
            _watchTask?.Dispose();
            _watcher?.Dispose();

            _watcher = new FileSystemWatcher(Path.GetDirectoryName(_currentFile)!);
            _watcher.Changed += (_, args) => {
                Console.WriteLine($"{args.FullPath} :: {args.ChangeType}");
                if (args.FullPath == _currentFile) {
                    Task.Run(async () => {
                        await Task.Delay(100);
                        LoadShaderFromFile(args.FullPath);
                    });
                }
            };
            _watcher.Deleted += (_, args) => {
                Console.WriteLine($"{args.FullPath} :: {args.ChangeType}");
                if (args.FullPath == _currentFile) {
                    _currentFile = null;
                }
            };

            _watchTask = Task.Run(() => _watcher.WaitForChanged(WatcherChangeTypes.Changed | WatcherChangeTypes.Deleted));
        }
    }

    protected override void OnFileDrop(FileDropEventArgs e) {
        base.OnFileDrop(e);
        foreach (string fileName in e.FileNames) {
            if (fileName.EndsWith(".frag")) {
                _currentFile = fileName;
                LoadShaderFromFile(fileName);
            }
        }
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e) {
        base.OnKeyDown(e);
        if (e.Key == Keys.Escape) {
            Close();
        } else if (e is { Key: Keys.Enter, Alt: true }) {
            WindowState = IsFullscreen ? WindowState.Normal : WindowState.Fullscreen;
        }
    }
}
