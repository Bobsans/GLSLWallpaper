using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using GLSLWallpapers.Display;
using GLSLWallpapers.Helpers;

namespace GLSLWallpapers {
    static class Program {
        [STAThread]
        static void Main(string[] args) {
            Logger.FilePath = "./latest.log";

            ShaderRegistry.Load();
            Config.Load();

            ProcessArguments(args);

            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            Application.Run(new Context());

            Config.Save();
        }

        static void ProcessArguments(IReadOnlyList<string> args) {
            if (args.Count == 2) {
                if (args[0] == "-install") {
                    ShaderRegistry.Install(args[1]);
                }
            }
        }
    }

    class Context : ApplicationContext {
        static SettingsForm settings_form;
        readonly NotifyIcon icon;

        public Context() {
            icon = new NotifyIcon {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),

                ContextMenu = new ContextMenu(new[] {
                    new MenuItem("Settings", (sender, args) => ShowSettingsForm()),
                    new MenuItem("-"),
                    new MenuItem("Exit", (sender, args) => Exit())
                }),
                Visible = true
            };

            icon.DoubleClick += (sender, args) => ShowSettingsForm();

            new Thread(() => new DisplayWindow().Run(Config.UpdatesPerSecond, Config.FramesPerSecond)).Start();
        }

        static void ShowSettingsForm() {
            if (settings_form == null) {
                settings_form = new SettingsForm();
                settings_form.Show();
                settings_form.Closing += (sender, args) => settings_form = null;
            } else {
                settings_form.Focus();
            }
        }

        void Exit() {
            icon.Visible = false;
            DisplayWindow.Instance.Close();
            ExitThread();
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            icon.Dispose();
        }
    }
}
