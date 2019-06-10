using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CUnit.Entities
{
    class TestInfo
    {
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public MethodInfo InitMethod { get; set; }
        public List<MethodInfo> TestMethods { get; set; }
    }
}
