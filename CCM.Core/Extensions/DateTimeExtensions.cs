using System;

namespace CCM.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime Max(this DateTime dateTime, DateTime otherDateTime)
        {
            return dateTime >= otherDateTime ? dateTime : otherDateTime;
        }
    }
}