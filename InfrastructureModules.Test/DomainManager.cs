using InfrastructureModules.Test.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using InfrastructureUtils = InfrastructureModules.Utils;

namespace InfrastructureModules.Test
{
    class DomainManager
    {
        public Dictionary<string, DomainInfo> PrepareAppDomains()
        {
            Dictionary<string, DomainInfo> domainMap = new Dictionary<string, DomainInfo>();
            List<AssemblyInfo> assemblies = InfrastructureUtils.SingletonProvider<Configuration>.Instance.GetAssemblies();
            foreach (AssemblyInfo assemblyInfo in assemblies)
            {
                if (!domainMap.ContainsKey(assemblyInfo.ProjectName))
                {
                    assemblyInfo.AssemblyFullPath = CalculateAssemblyFullPath(assemblyInfo);
                    AppDomainSetup appDomainSetup = new AppDomainSetup()
                    {
                        ApplicationBase = System.Environment.CurrentDirectory,
                        PrivateBinPath = assemblyInfo.AssemblyFullPath
                    };
                    Evidence adEvidence = AppDomain.CurrentDomain.Evidence;
                    AppDomain newDomain = AppDomain.CreateDomain(assemblyInfo.ProjectName, adEvidence, appDomainSetup);
                    Type type = typeof(AssemblyLoader);
                    AssemblyLoader assemblyLoader = (AssemblyLoader)newDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                    assemblyLoader.LoadAssembly(string.Format("{0}\\{1}.{2}", assemblyInfo.AssemblyFullPath, assemblyInfo.AssemblyName, assemblyInfo.Extension));

                    DomainInfo domainInfo = new DomainInfo()
                    {
                        AppDomain = newDomain,
                        Assemblies = new List<AssemblyInfo>()
                    };

                    domainMap.Add(assemblyInfo.ProjectName, domainInfo);
                }

                domainMap[assemblyInfo.ProjectName].Assemblies.Add(assemblyInfo);
            }

            return domainMap;
        }

        private string CalculateAssemblyFullPath(AssemblyInfo assemblyInfo)
        {
            string projectFolderPath = string.IsNullOrWhiteSpace(assemblyInfo.ProjectFolderPath) ? string.Format("{0}{1}", Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\")), assemblyInfo.ProjectName) : assemblyInfo.ProjectFolderPath;

            string assemblyFullPath = string.Format(@"{0}\{1}", projectFolderPath, assemblyInfo.BinFolderPath);
            return assemblyFullPath;
        }
    }
}
