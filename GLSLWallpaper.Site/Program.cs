using DimTim.Configuration.Yaml;
using DimTim.Http;
using DimTim.Utils.DI;
using DimTim.Utils.Logging;
using GLSLWallpaper.Site;
using GLSLWallpaper.Site.Utils;

Config config = YamlConfigProvider.Load<Config>(Path.Join(Config.Root, "config.yaml"));

SC.AddSingleton(() => new Renderer(config.TemplatePath));

WebServer server = new(config.Host, config.Port) {
    Settings = {
        Logger = new ConsoleLogger(config.LogLevel)
    }
};

Console.WriteLine(server.Router.Routes);

await server.Start().ConfigureAwait(false);
