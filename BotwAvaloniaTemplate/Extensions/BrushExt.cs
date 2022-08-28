using Avalonia.Media;

namespace BotwAvaloniaTemplate.Extensions
{
    public static class BrushExt
    {
        public static Brush ToBrush(this string color) => (Brush)(new BrushConverter().ConvertFromString(color) ?? new());
        public static Brush? ToBrush(this bool? value)
        {
            return value switch {
                true => "#00CC1C".ToBrush(),
                false => "#FF0000".ToBrush(),
                _ => "#00000000".ToBrush(),
            };
        }
    }
}
