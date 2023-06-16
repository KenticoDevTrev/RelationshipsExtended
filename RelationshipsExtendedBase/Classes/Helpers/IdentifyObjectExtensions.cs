using System;
using System.Collections.Generic;
using System.Linq;

namespace system
{
    public static class RelExtendedIdentifyObjectExtensions
    {
        /// <summary>
        /// Cast string as an object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToObject(this string value)
        {
            return value;
        }

        /// <summary>
        /// Cast string array as an object array
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<object> ToObjectArray(this IEnumerable<string> values)
        {
            return values.Select(x => (object)x);
        }

        /// <summary>
        /// Cast int as an object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToObject(this int value)
        {
            return value;
        }

        /// <summary>
        /// Cast int array as object array
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<object> ToObjectArray(this IEnumerable<int> values)
        {
            return values.Select(x => (object)x);
        }

        /// <summary>
        /// Cast Guid as an object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToObject(this Guid value)
        {
            return value;
        }

        /// <summary>
        /// Cast Guid array as an object array
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<object> ToObjectArray(this IEnumerable<Guid> values)
        {
            return values.Select(x => (object)x);
        }



    }
}
