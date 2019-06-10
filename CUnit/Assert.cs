using CUnit.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUnit
{
    public static class Assert
    {
        public static void Equals(object target, object expected) {
            Equals<object>(target, expected);
        }

        public static void Equals(long target, long expected)
        {
            Equals<long>(target, expected, EqualityComparer<long>.Default, "Numbers Are Not Equal. Values: {0}, {1}");
        }

        public static void Equals(int target, int expected)
        {
            Equals<int>(target, expected, EqualityComparer<int>.Default, "Numbers Are Not Equal. Values: {0}, {1}");
        }

        public static void Equals<T>(T target, T expected, string message = "Values Are Not Equal. Values: {0}, {1}")
        {
            Equals<T>(target, expected, EqualityComparer<T>.Default, "Numbers Are Not Equal. Values: {0}, {1}");
        }

        public static void Equals<T>(T target, T expected, IEqualityComparer<T> comparer, string message = "Values Are Not Equal. Values: {0}, {1}")
        {
            List<object> parameters = new List<object>() { target, expected };
            if(!comparer.Equals(target, expected)) {
                throw new AssertionException(string.Format(message, target, expected), AssertionType.EqualityCheck, parameters);
            }
        }
    }
}
