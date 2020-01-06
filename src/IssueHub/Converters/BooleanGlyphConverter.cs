using System;
using System.Globalization;
using IssueHub.Utils;
using Xamarin.Forms;

namespace IssueHub.Converters
{
    public sealed class BooleanGlyphConverter : IValueConverter
    {
        public static readonly BooleanGlyphConverter Default = new BooleanGlyphConverter();

        BooleanGlyphConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b)
            {
                return ((char)(int)Octicons.Glyph.Check).ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
