using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            string testProjectsJson = File.ReadAllText(string.Format(@"{0}\..\..\TestProjects.json", Directory.GetCurrentDirectory()));
            TestProjectConfig testProjectConfig = JsonConvert.DeserializeObject<TestProjectConfig>(testProjectsJson);
            assemblies = testProjectConfig.Projects;
        }

        public List<AssemblyInfo> GetAssemblies()
        {
            return assemblies;
        }
    }
}
