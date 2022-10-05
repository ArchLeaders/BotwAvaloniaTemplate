namespace BotwAvaloniaTemplate
{
    public static class Meta
    {
        public static string Name { get; } = "$safeprojectname$";
        public static string Version { get; } = "0.1.0-alpha";
        public static string Footer { get; } = $"{Name} — v{Version}";
        public static string BaseUrl { get; } = "https://raw.githubusercontent.com/ArchLeaders/$safeprojectname$/master";
        public static string ToCommonPath(this string path) => path.Replace("\\", "/");
    }
}
