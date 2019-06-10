using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUnit
{
    [Serializable]
    class AssemblyInfo
    {
        public string AssemblyName { get; set; }
        public string AssemblyFullPath { get; set; }
        public string Extension { get; set; }
        public string ProjectName { get; set; }
        public string BinFolderPath { get; set; }
        public string ProjectFolderPath { get; set; }
    }
}
