using System;
using System.Diagnostics;
using System.Text;

namespace IssueHub.Utils
{
    public static class Logger
    {
        public static void LogError(Exception ex, string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine(message);
            if (ex != null)
            {
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
            }
            Trace.TraceError(ex.ToString());
        }

        public static void LogWarning(string message) =>
            Trace.TraceWarning(message);

        public static void LogWarning(Exception ex, string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine(message);
            if (ex != null)
            {
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
            }
            Trace.TraceWarning(sb.ToString());
        }
    }
}
