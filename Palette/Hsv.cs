namespace Palette;

using System;
using System.Windows.Media;

public struct Hsv
{
    public Hsv(double hue, double saturation, double value)
    {
        this.Hue = hue;
        this.Saturation = saturation;
        this.Value = value;
    }

    public double Hue { get; }

    public double Saturation { get; }

    public double Value { get; }

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
        var hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        var f = (hue / 60) - Math.Floor(hue / 60);

        value = value * 255;
        var v = (byte)value;
        var p = (byte)(value * (1 - saturation));
        var q = (byte)(value * (1 - (f * saturation)));
        var t = (byte)(value * (1 - ((1 - f) * saturation)));

        if (hi == 0)
        {
            return Color.FromArgb(255, v, t, p);
        }
        else if (hi == 1)
        {
            return Color.FromArgb(255, q, v, p);
        }
        else if (hi == 2)
        {
            return Color.FromArgb(255, p, v, t);
        }
        else if (hi == 3)
        {
            return Color.FromArgb(255, p, q, v);
        }
        else if (hi == 4)
        {
            return Color.FromArgb(255, t, p, v);
        }
        else
        {
            return Color.FromArgb(255, v, p, q);
        }
    }
}
