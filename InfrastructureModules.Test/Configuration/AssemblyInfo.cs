using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test
{
    [Serializable]
    class AssemblyInfo
    {
        public string AssemblyName { get; set; }
        public string AssemblyFullPath { get; set; }
        public string Extension { get; set; }
        public string ProjectName { get; set; }

        /// <summary>
        /// If assemly is not in the same solution, path will be used.
        /// </summary>
        public string BinFolderPath { get; set; }
        public string ProjectFolderPath { get; set; }
    }
}
