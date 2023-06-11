namespace GLSLWallpaper;

public static class Identity {
    public const string GUID = "72f9128b-0bcd-3b37-b98d-939efaf33f32";

    public static readonly string NAME = typeof(Identity).Assembly.GetName().Name!;
    public static readonly string VERSION = typeof(Identity).Assembly.GetName().Version!.ToString();
}
