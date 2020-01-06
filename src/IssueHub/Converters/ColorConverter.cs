using System;
using System.Globalization;
using Octokit;
using Xamarin.Forms;

namespace IssueHub.Converters
{
    public sealed class ColorConverter : IValueConverter
    {
        public static readonly ColorConverter Default = new ColorConverter();

        ColorConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case string hex:
                    return Color.FromHex(hex);
                case StringEnum<ItemState> state:
                    return ItemStateToColor(state.Value);
                case ItemState state:
                    return ItemStateToColor(state);
                default:
                    return Color.Default;
            }
        }

        static readonly Color OpenColor = Color.FromHex("2cbe4e");

        static readonly Color ClosedColor = Color.FromHex("cb2431");

        static Color ItemStateToColor(ItemState state)
        {
            if (state == ItemState.Open)
            {
                return OpenColor;
            }
            else if (state == ItemState.Closed)
            {
                return ClosedColor;
            }
            else
            {
                return Color.Default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
