using CUnit.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUnit
{
    class TestManager
    {
        public Dictionary<string, List<ClassTestResult>> Run()
        {
            DomainManager domainManager = new DomainManager();
            Dictionary<string, DomainInfo> projectMap = domainManager.PrepareAppDomains();
            ConcurrentDictionary<string, List<ClassTestResult>> projectTestResults = new ConcurrentDictionary<string, List<ClassTestResult>>();
            Parallel.ForEach(projectMap, (projectInfo) =>
            {
                DomainInfo domainInfo = projectInfo.Value;
                AppDomain domain = domainInfo.AppDomain;
                Type testRunnerType = typeof(TestRunner);
                TestRunner testRunner = (TestRunner)domain.CreateInstanceAndUnwrap(testRunnerType.Assembly.FullName, testRunnerType.FullName);

                List<ClassTestResult> testResults = testRunner.Run(domainInfo);
                projectTestResults.TryAdd(projectInfo.Key, testResults);
                AppDomain.Unload(domain);
            });

            return projectTestResults.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
