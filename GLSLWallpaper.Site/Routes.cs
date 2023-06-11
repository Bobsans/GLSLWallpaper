using DimTim.Http;
using DimTim.Utils.DI;
using GLSLWallpaper.Site.Utils;
using JetBrains.Annotations;

namespace GLSLWallpaper.Site;

[RouteCollection("/"), PublicAPI]
public static class Routes {
    [Get("/")]
    public static Response Index(Request request) {
        return new HtmlResponse(SC.Get<Renderer>().Render("index", new { message = "IDI NAHUI LOH!" }));
    }
}
