// ReSharper disable PossibleNullReferenceException
namespace Palette;

using System;
using System.Globalization;
using System.Windows.Data;

[ValueConversion(typeof(double), typeof(double))]
public sealed class NegateConverter : IValueConverter
{
    public static readonly NegateConverter Default = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return -1 * (double)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return -1 * (double)value;
    }
}
