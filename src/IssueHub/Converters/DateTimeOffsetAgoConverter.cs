using System;
using System.Globalization;
using Xamarin.Forms;

namespace IssueHub.Converters
{
    public sealed class DateTimeOffsetAgoConverter : IValueConverter
    {
        public static readonly DateTimeOffsetAgoConverter Default = new DateTimeOffsetAgoConverter();

        static readonly CultureInfo enUSCulter = CultureInfo.CreateSpecificCulture("en-US");

        DateTimeOffsetAgoConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dto)
            {
                return GetAgoText(dto);
            }
            return value;
        }

        static string GetAgoText(DateTimeOffset dateTimeOffset)
        {
            var now = DateTimeOffset.Now;
            var dto = dateTimeOffset.ToUniversalTime().ToOffset(now.Offset);
            var diff = now - dto;
            if (diff.Days == 0)
            {
                if (diff.Hours == 0)
                {
                    if (diff.Minutes == 0)
                    {
                        return $"{diff.Seconds} seconds ago";
                    }
                    else
                    {
                        return $"{diff.Minutes} minutes ago";
                    }
                }
                else
                {
                    return $"{diff.Hours} hours ago";
                }
            }
            else if (diff.Days == 1)
            {
                return "yesterday";
            }
            else
            {
                if (diff.Days < 30)
                {
                    return $"{diff.Days} days ago";
                }
                else if (diff.Days < 365)
                {
                    return $"on {dto.ToString("d MMM", enUSCulter)}";
                }
                else
                {
                    return $"on {dto.ToString("d MMM yyyy", enUSCulter)}";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
