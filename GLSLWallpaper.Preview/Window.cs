using System.Collections.Concurrent;
using System.Drawing.Imaging;
using GLSLWallpaper.Common;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace GLSLWallpaper.Preview;

public sealed class Window : ShaderWindow {
    static readonly ConcurrentQueue<string?> _queue = new();
    readonly bool _watch;
    double _time;
    string? _currentFile;
    FileSystemWatcher? _watcher;
    readonly ErrorForm _errorForm;

    public Window(string? file, bool watch = false) : base(new NativeWindowSettings {
        Size = new Vector2i(960, 540),
        Icon = GetIcon(),
        Title = Identity.NAME + Identity.Version
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
            if (_watcher == null) {
                _watcher = new FileSystemWatcher(Path.GetDirectoryName(_currentFile)!) {
                    EnableRaisingEvents = true
                };
                _watcher.Changed += (_, args) => {
                    if (args.FullPath == _currentFile) {
                        Task.Run(async () => {
                            await Task.Delay(100);
                            LoadShaderFromFile(args.FullPath);
                        });
                    }
                };
                _watcher.Deleted += (_, args) => {
                    if (args.FullPath == _currentFile) {
                        _currentFile = null;
                    }
                };
                _watcher.Renamed += (_, args) => {
                    if (args.OldFullPath == _currentFile) {
                        LoadShaderFromFile(args.FullPath);
                    }
                };
            } else {
                _watcher.Path = Path.GetDirectoryName(_currentFile)!;
            }
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

    static WindowIcon GetIcon() {
        Bitmap icon = System.Drawing.Icon.ExtractAssociatedIcon(typeof(Window).Assembly.Location)!.ToBitmap();
        BitmapData bits = icon.LockBits(new Rectangle(0, 0, icon.Width, icon.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        byte[] bytes = new byte[icon.Width * icon.Height * 4];
        System.Runtime.InteropServices.Marshal.Copy(bits.Scan0, bytes, 0, bytes.Length);
        return new WindowIcon(new OpenTK.Windowing.Common.Input.Image(icon.Width, icon.Height, bytes));
    }
}
