using Avalonia.Media;

namespace BotwAvaloniaTemplate.Extensions
{
    public static class BrushExt
    {
        public static Brush ToBrush(this string color) => (Brush)(new BrushConverter().ConvertFromString(color) ?? new());
        public static Brush? GetBrush(this string brush)
        {
            if (Avalonia.Application.Current != null) {
                Avalonia.Application.Current.Resources.TryGetResource(brush, out object? obj);
                return obj as Brush;
            }
            else {
                return null;
            }
        }
    }
}
