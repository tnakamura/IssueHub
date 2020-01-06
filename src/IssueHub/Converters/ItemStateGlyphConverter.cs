using System;
using System.Globalization;
using Xamarin.Forms;
using IssueHub.Utils;
using Octokit;

namespace IssueHub.Converters
{
    public sealed class ItemStateGlyphConverter : IValueConverter
    {
        public static readonly ItemStateGlyphConverter Default = new ItemStateGlyphConverter();

        ItemStateGlyphConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StringEnum<ItemState> a)
            {
                return ItemStateToGlyph(a.Value);
            }
            if (value is ItemState b)
            {
                return ItemStateToGlyph(b);
            }
            return value;
        }

        static readonly string OpenGlyph = ((char)(int)Octicons.Glyph.IssueOpened).ToString();

        static readonly string ClosedGlyph = ((char)(int)Octicons.Glyph.IssueClosed).ToString();

        static string ItemStateToGlyph(ItemState state)
        {
            if (state == ItemState.Open)
            {
                return OpenGlyph;
            }
            else if (state == ItemState.Closed)
            {
                return ClosedGlyph;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
