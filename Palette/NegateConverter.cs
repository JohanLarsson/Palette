// ReSharper disable PossibleNullReferenceException
namespace Palette
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NegateConverter : IValueConverter
    {
        public static readonly NegateConverter Default = new NegateConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -1 * (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -1 * (double)value;
        }
    }
}
