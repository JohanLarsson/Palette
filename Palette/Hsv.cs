namespace Palette;

using System;
using System.Windows.Media;

public record struct Hsv(double Hue, double Saturation, double Value)
{
    public static Hsv ColorToHSV(Color color)
    {
        return ColorToHSV(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
    }

    // http://stackoverflow.com/a/1626175/1069200
    public static Hsv ColorToHSV(System.Drawing.Color color)
    {
        int max = Math.Max(color.R, Math.Max(color.G, color.B));
        int min = Math.Min(color.R, Math.Min(color.G, color.B));

        var hue = color.GetHue();
        var saturation = max == 0 ? 0 : 1d - (1d * min / max);
        var value = max / 255d;
        return new Hsv(hue, saturation, value);
    }

    public static Color ColorFromHSV(double hue, double saturation, double value)
    {
        var hi = (int)Math.Floor(hue / 60) % 6;
        var f = (hue / 60) - Math.Floor(hue / 60);

        value *= 255;
        var v = (byte)value;
        var p = (byte)(value * (1 - saturation));
        var q = (byte)(value * (1 - (f * saturation)));
        var t = (byte)(value * (1 - ((1 - f) * saturation)));

        return hi switch
        {
            0 => Color.FromArgb(255, v, t, p),
            1 => Color.FromArgb(255, q, v, p),
            2 => Color.FromArgb(255, p, v, t),
            3 => Color.FromArgb(255, p, q, v),
            4 => Color.FromArgb(255, t, p, v),
            _ => Color.FromArgb(255, v, p, q),
        };
    }
}
