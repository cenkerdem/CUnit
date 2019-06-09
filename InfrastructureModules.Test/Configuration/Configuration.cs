using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core = InfrastructureModules.Utils;

namespace InfrastructureModules.Test
{
    class Configuration
    {
        private List<AssemblyInfo> assemblies;
        public Configuration() {
            assemblies = new List<AssemblyInfo>();
            assemblies.Add(new AssemblyInfo() {
                AssemlyName = "SampleLibrary",
                Extension = "dll",
                ProjectName = "SampleLibrary",
                BinFolderPath = "bin\\debug"
            });
        }

        public List<AssemblyInfo> GetAssemblies()
        {
            return assemblies;
        }
    }
}
