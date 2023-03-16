// ReSharper disable PossibleNullReferenceException
namespace Palette;

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

[ValueConversion(typeof(double), typeof(double))]
[MarkupExtensionReturnType(typeof(SaturationToRadiusConverter))]
public class SaturationToRadiusConverter : MarkupExtension, IValueConverter
{
    public double Radius { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (double)value * this.Radius;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (double)value / this.Radius;
    }
}
