using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using GLSLWallpapers.Display;
using GLSLWallpapers.Helpers;

namespace GLSLWallpapers {
    static class Program {
        [STAThread]
        static void Main() {
            Logger.FilePath = "./latest.log";

            ShaderRegistry.Load();
            Config.Load();

            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            Application.Run(new Context());

            Config.Save();
        }
    }

    class Context : ApplicationContext {
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

            new Thread(() => new DisplayWindow().Run(60.0f, Config.UpdateFrequency)).Start();
        }

        static void ShowSettingsForm() {
            new SettingsForm().Show();
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