using System;

namespace SeaBattle.Common.Extensions
{
    public static class AssertExtensions
    {
        public static T NotNull<T>(this T source, string paramName)
        {
            if (source is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return source;
        }
    }
}
