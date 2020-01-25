using System.Diagnostics;
using System.IO;
using GLSLWallpapers.Helpers;
using SciterSharp;
using SciterSharp.Interop;

namespace GLSLWallpapers {
    class Host : BaseHost {
        public Host(SciterWindow window) {
            Setup(window);
            AttachEvh(new HostEvh(this));
            SetupPage("index.html");

#if DEBUG
            try {
                DebugInspect();
            } catch {
                // ignore
            }
#endif

            window.Show();
        }
    }

    class HostEvh : SciterEventHandler {
        static Host Host { get; set; }

        public HostEvh(Host host) {
            Host = host;
        }

        protected override bool OnScriptCall(SciterElement se, string name, SciterValue[] args, out SciterValue result) {
            result = null;
            return name switch {
                "LoadConfigs" => SendConfigs(args[0]),
                "LoadShaders" => SendShaders(args[0]),
                "ConfigSetInteger" => SetConfigValue(args[0].Get(""), args[1].Get(0), args.Length > 2 ? args[2] : SciterValue.Null),
                "ConfigSetDouble" => SetConfigValue(args[0].Get(""), args[1].Get(0D), args.Length > 2 ? args[2] : SciterValue.Null),
                "ConfigSetBoolean" => SetConfigValue(args[0].Get(""), args[1].Get(false), args.Length > 2 ? args[2] : SciterValue.Null),
                "ConfigSetString" => SetConfigValue(args[0].Get(""), args[1].Get(""), args.Length > 2 ? args[2] : SciterValue.Null),
                _ => false
            };
        }

        static bool SendConfigs(SciterValue callback) {
            if (callback.IsObjectFunction) {
                Host.InvokePost(() => { callback.Call(Config.SciterValue); });
            }

            return true;
        }

        static bool SendShaders(SciterValue callback) {
            if (callback.IsObjectFunction) {
                Host.InvokePost(() => { callback.Call(ShaderRegistry.SciterValue); });
            }

            return true;
        }

        static bool SetConfigValue<T>(string name, T value, SciterValue callback) {
            Config.SetFieldValue(name, value);
            if (callback.IsObjectFunction) {
                Host.InvokePost(() => { callback.Call(new SciterValue(true)); });
            }

            return true;
        }
    }

    class BaseHost : SciterHost {
        SciterWindow window;
        static readonly SciterArchive archive = new SciterArchive();
#if DEBUG
        static readonly string rescwd;
#endif
        static BaseHost() {
#if DEBUG
            rescwd = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "../../../Sources/");
            rescwd = Path.GetFullPath(rescwd).Replace('\\', '/');
            Debug.Assert(Directory.Exists(rescwd));
#else
            archive.Open(ArchiveResource.Resources);
#endif
        }

        protected void Setup(SciterWindow wnd) {
            window = wnd;
            SetupWindow(wnd);
        }

        protected void SetupPage(string pageFromResFolder) {
#if DEBUG
            string path = rescwd + pageFromResFolder;
            Debug.Assert(File.Exists(path));
            string url = "file://" + path;
#else
            string url = "archive://app/" + pageFromResFolder;
#endif
            window.LoadPage(url);
        }

        protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld) {
            if (sld.uri.StartsWith("archive://app/")) {
                string path = sld.uri.Substring(14);
                byte[] data = archive.Get(path);
                if (data != null)
                    SciterX.API.SciterDataReady(sld.hwnd, sld.uri, data, (uint)data.Length);
            }

            return base.OnLoadData(sld);
        }
    }
}
