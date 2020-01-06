using System;
using System.Globalization;
using Xamarin.Forms;

namespace IssueHub.Converters
{
    public sealed class IntegerBooleanConverter : IValueConverter
    {
        public static readonly IntegerBooleanConverter Default = new IntegerBooleanConverter();

        IntegerBooleanConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
            {
                return 0 < i;
            }
            if (value is long l)
            {
                return 0 < l;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
