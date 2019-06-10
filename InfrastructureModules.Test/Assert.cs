using InfrastructureModules.Test.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test
{
    public static class Assert
    {
        public static void Equals(object first, object second) {
            if (!first.Equals(second))
            {
                List<object> parameters = new List<object>() { first, second };
                throw new AssertionException("Objects Are Not Equal", AssertionType.EqualityCheck, parameters);
            }
        }

        public static void Equals(long first, long second)
        {
            if (!first.Equals(second))
            {
                List<object> parameters = new List<object>() { first, second };
                throw new AssertionException(string.Format("Numbers Are Not Equal. Values: {0}, {1}", first, second), AssertionType.EqualityCheck, parameters);
            }
        }

        public static void Equals(int first, int second)
        {
            if (!first.Equals(second))
            {
                List<object> parameters = new List<object>() { first, second };
                throw new AssertionException(string.Format("Numbers Are Not Equal. Values: {0}, {1}", first, second), AssertionType.EqualityCheck, parameters);
            }
        }
    }
}
