using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using GLSLWallpaper.UI;
using Hardcodet.Wpf.TaskbarNotification;

namespace GLSLWallpaper;

public partial class App {
    static readonly Mutex _mutex = new(true, Identity.GUID);

    public App() {
        AppDomain.CurrentDomain.UnhandledException += (_, args) => {
            Logger.Error(args.ExceptionObject.ToString() ?? "Unknown error");
        };
    }

    protected override void OnStartup(StartupEventArgs e) {
        ProcessArguments(e.Args);
        
        if (_mutex.WaitOne(TimeSpan.Zero, true)) {
            base.OnStartup(e);
            
            Settings.Load();
            PackRegistry.Load();

            _ = new TaskbarIcon {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Visibility = Visibility.Visible,
                ContextMenu = (ContextMenu)FindResource("MainContextMenu")!,
                MenuActivation = PopupActivationMode.RightClick
            };
            
            BackgroundWindow.RunThreaded();
            BackgroundWindow.EnqueueShader(PackRegistry.GetByHash(Settings.CurrentShader, () => PackRegistry.Packs.First()));
            Win32.SetWindowAsDesktopChild(Process.GetCurrentProcess().MainWindowHandle);
            PipeWorker.RunServer();
        } else {
            Current.Shutdown();
        }
    }

    protected override void OnExit(ExitEventArgs e) {
        base.OnExit(e);
        Win32.RefreshWallpaper();
    }

    void MenuItemSettings_OnClick(object sender, RoutedEventArgs e) => new MainWindow().Show();
    void MenuItemExit_OnClick(object sender, RoutedEventArgs e) => Current.Shutdown();

    static void ProcessArguments(IReadOnlyList<string> args) {
        if (args.Count == 1) {
            string path = Path.GetFullPath(args[0]);
            if (PackRegistry.IsShaderPackFile(path)) {
                PackRegistry.Install(path);
            }
        }
    }
}
