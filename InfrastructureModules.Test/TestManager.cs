using InfrastructureModules.Test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test
{
    class TestManager
    {
        public Dictionary<string, List<ClassTestResult>> Run()
        {
            DomainManager domainManager = new DomainManager();
            Dictionary<string, DomainInfo> projectMap = domainManager.PrepareAppDomains();
            Dictionary<string, List<ClassTestResult>> projectTestResults = new Dictionary<string, List<ClassTestResult>>();
            Parallel.ForEach(projectMap, (projectInfo) => {
                DomainInfo domainInfo = projectInfo.Value;
                AppDomain domain = domainInfo.AppDomain;
                Type testRunnerType = typeof(TestRunner);
                TestRunner testRunner = (TestRunner)domain.CreateInstanceAndUnwrap(testRunnerType.Assembly.FullName, testRunnerType.FullName);

                List<ClassTestResult> testResults = testRunner.Run(domainInfo);
                projectTestResults.Add(projectInfo.Key, testResults);
                AppDomain.Unload(domain);
            });
            //foreach (var pair in projectMap)
            //{
            //    DomainInfo domainInfo = pair.Value;
            //    AppDomain domain = domainInfo.AppDomain;
            //    Type testRunnerType = typeof(TestRunner);
            //    TestRunner testRunner = (TestRunner)domain.CreateInstanceAndUnwrap(testRunnerType.Assembly.FullName, testRunnerType.FullName);

            //    List<TestResult> testResults = testRunner.Run(domainInfo);
            //    projectTestResults.Add(pair.Key, testResults);
            //    AppDomain.Unload(domain);
            //}

            return projectTestResults;
        }
    }
}
