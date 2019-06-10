using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUnit.Exceptions
{
    public class AssertionException : Exception
    {
        public AssertionType AssertionType { get; set; }
        public List<object> Parameters { get; set; }

        public AssertionException(string message, AssertionType assertionType, List<object> parameters)
            : base(message)
        {
            AssertionType = assertionType;
            Parameters = parameters;
        }
    }
}
