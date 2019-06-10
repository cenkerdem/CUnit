using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUnit.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClass : Attribute
    {
        public string Description { get; set; }

        public TestClass(string description)
        {
            Description = description;
        }
    }
}
