using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test.Entities
{
    class TestInfo
    {
        public Type ClassType { get; set; }
        public MethodInfo InitMethod { get; set; }
        public List<MethodInfo> TestMethods { get; set; }
    }
}
