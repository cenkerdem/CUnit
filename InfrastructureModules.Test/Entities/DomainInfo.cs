using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUnit.Entities
{
    [Serializable]
    class DomainInfo
    {
        public AppDomain AppDomain { get; set; }
        public List<AssemblyInfo> Assemblies { get; set; }
        public Dictionary<Type, TestInfo> TestClasses { get; set; }
    }
}
