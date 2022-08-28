namespace BotwAvaloniaTemplate
{
    internal static class Meta
    {
        public static string Version { get; } = "0.1.0-alpha";
        public static string BaseUrl { get; } = "https://raw.githubusercontent.com/ArchLeaders/BotwAvaloniaTemplate/master";
        public static string ToCommonPath(this string path) => path.Replace("\\", "/");
    }
}
