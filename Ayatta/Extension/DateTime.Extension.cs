using System;

namespace Ayatta.Extension
{
    public static partial class Common
    {
        public static string ToString(this DateTime? dateTime, string format, string @default)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(format) : @default;
        }

        public static string ToString(this DateTime? dateTime, string format, DateTime @default)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(format) : @default.ToString(format);
        }
    }
}