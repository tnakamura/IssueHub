using System;
using System.Globalization;
using Octokit;
using Xamarin.Forms;

namespace IssueHub.Converters
{
    public sealed class TextDecorationsConverter : IValueConverter
    {
        public static readonly TextDecorationsConverter Default = new TextDecorationsConverter();

        TextDecorationsConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StringEnum<ItemState> a)
            {
                return ItemStateToTextDecorations(a.Value);
            }
            if (value is ItemState b)
            {
                return ItemStateToTextDecorations(b);
            }
            return value;
        }

        static TextDecorations ItemStateToTextDecorations(ItemState state)
        {
            if (state == ItemState.Closed)
            {
                return TextDecorations.Strikethrough;
            }
            else
            {
                return TextDecorations.None;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
