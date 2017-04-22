namespace Palette
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    public class SaturationToRadiusConverter : MarkupExtension , IValueConverter
    {
        public double Radius { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double) value * Radius;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / Radius;
        }
    }
}