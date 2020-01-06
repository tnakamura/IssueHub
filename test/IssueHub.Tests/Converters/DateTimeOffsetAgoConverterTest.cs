using System;
using System.Globalization;
using IssueHub.Converters;
using Xunit;

namespace IssueHub.Tests
{
    public class DateTimeOffsetAgoConverterTest
    {
        readonly DateTimeOffsetAgoConverter converter = DateTimeOffsetAgoConverter.Default;

        [Fact]
        public void ConvertYesterdayTest()
        {

            Assert.Equal(
                "yesterday",
                converter.Convert(
                    DateTimeOffset.Now.AddDays(-1),
                    typeof(DateTimeOffset),
                    null,
                    CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertDaysAgoTest()
        {
            Assert.Equal(
                "2 days ago",
                converter.Convert(
                    DateTimeOffset.Now.AddDays(-2),
                    typeof(DateTimeOffset),
                    null,
                    CultureInfo.CurrentCulture));
            Assert.Equal(
                "29 days ago",
                converter.Convert(
                    DateTimeOffset.Now.AddDays(-29),
                    typeof(DateTimeOffset),
                    null,
                    CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertDayMonthTest()
        {
            var dto = DateTimeOffset.Now.AddDays(-31);
            var culter = CultureInfo.CreateSpecificCulture("en-US");
            Assert.Equal(
                $"on {dto.ToString("d MMM", culter)}",
                converter.Convert(
                    dto,
                    typeof(DateTimeOffset),
                    null,
                    CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertHoursAgoTest()
        {
            Assert.Equal(
                "1 hours ago",
                converter.Convert(
                    DateTimeOffset.Now.AddHours(-1),
                    typeof(DateTimeOffset),
                    null,
                    CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertMinutesAgoTest()
        {
            Assert.Equal(
                "1 minutes ago",
                converter.Convert(
                    DateTimeOffset.Now.AddMinutes(-1),
                    typeof(DateTimeOffset),
                    null,
                    CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertSecondsAgoTest()
        {
            Assert.Equal(
                "1 seconds ago",
                converter.Convert(
                    DateTimeOffset.Now.AddSeconds(-1),
                    typeof(DateTimeOffset),
                    null,
                    CultureInfo.CurrentCulture));
        }
    }
}
