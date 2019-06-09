using InfrastructureModules.Test.Attributes;
using InfrastructureModules.Test.Entities;
using InfrastructureModules.Test.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test
{
    class TestManager
    {   
        public Dictionary<string, List<TestResult>> Run()
        {
            DomainManager domainManager = new DomainManager();
            Dictionary<string, DomainInfo> projectMap = domainManager.PrepareAppDomains();
            Dictionary<string, List<TestResult>> projectTestResults = new Dictionary<string, List<TestResult>>();
            foreach (var pair in projectMap)
            {
                DomainInfo domainInfo = pair.Value;
                AppDomain domain = domainInfo.AppDomain;
                Type testRunnerType = typeof(TestRunner);
                TestRunner testRunner = (TestRunner)domain.CreateInstanceAndUnwrap(testRunnerType.Assembly.FullName, testRunnerType.FullName);

                List<TestResult> testResults = testRunner.Run(domainInfo);
                projectTestResults.Add(pair.Key, testResults);
                AppDomain.Unload(domain);
            }

            return projectTestResults;
        }
    }

    class DomainManager
    {
        public Dictionary<string, DomainInfo> PrepareAppDomains()
        {
            Dictionary<string, DomainInfo> domainMap = new Dictionary<string, DomainInfo>();
            List<AssemblyInfo> assemblies = Utils.SingletonProvider<Configuration>.Instance.GetAssemblies();
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
                    assemblyLoader.LoadAssembly(string.Format("{0}\\{1}.{2}", assemblyInfo.AssemblyFullPath, assemblyInfo.AssemlyName, assemblyInfo.Extension));

                    DomainInfo domainInfo = new DomainInfo()
                    {
                        AppDomain = newDomain,
                        Assemblies = new List<AssemblyInfo>()
                    };

                    domainMap.Add(assemblyInfo.ProjectName, domainInfo);
                }

                domainMap[assemblyInfo.ProjectName].Assemblies.Add(assemblyInfo);

                //newDomain.Load(AssemblyName.GetAssemblyName(string.Format("{0}\\{1}", assemblyFullPath, assemblyInfo.AssemlyName)));
                //Assembly assembly = Assembly.LoadFrom(string.Format("{0}\\{1}", assemblyFullPath, assemblyInfo.AssemlyName));
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

    public class AssemblyLoader : MarshalByRefObject
    {
        public void LoadAssembly(string assemblyPath)
        {
            try
            {
                Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    class TestRunner : MarshalByRefObject
    {
        public TestRunner()
        {
        }

        public List<TestResult> Run(DomainInfo domainInfo)
        {
            AnalyzeTestMethods(domainInfo);
            return Execute(domainInfo);
        }

        private void AnalyzeTestMethods(DomainInfo domainInfo)
        {
            //AAA KALDIĞIM YER: ASSEMBLY ISMINI BURAYA GONDERIP, SADECE O ASSEMBLY'LERE BAKMAM GEREKIYOR!
            //Type[] types = appDomain.GetAssemblies()[0].GetTypes();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            domainInfo.TestClasses = new Dictionary<Type, TestInfo>();
            foreach (Assembly assembly in assemblies)
            {
                string assemblyName = assembly.GetName().Name;
                bool isTestAssembly = false;
                foreach (AssemblyInfo testAssembly in domainInfo.Assemblies)
                {
                    string testAssemblyName = testAssembly.AssemlyName;
                    if (assemblyName.Equals(testAssemblyName))
                    {
                        isTestAssembly = true;
                        break;
                    }
                }

                if (!isTestAssembly)
                {
                    continue;
                }

                Type[] types = assembly.GetTypes();

                //Type[] types = this.GetType().Assembly.GetTypes();
                //Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    Attribute attribute = type.GetCustomAttribute(typeof(TestClass));
                    if (attribute == null)
                    {
                        continue;
                    }

                    TestInfo testInfo = new TestInfo()
                    {
                        AssemblyName = type.Assembly.FullName,
                        ClassName  = type.Name
                    };

                    List<MethodInfo> testMethods = new List<MethodInfo>();

                    BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.ExactBinding;

                    MethodInfo[] methods = type.GetMethods(flags);
                    foreach (MethodInfo method in methods)
                    {
                        attribute = method.GetCustomAttribute(typeof(InitMethod));
                        if (attribute != null)
                        {
                            testInfo.InitMethod = method;
                            continue;
                        }

                        attribute = method.GetCustomAttribute(typeof(TestMethod));
                        if (attribute != null)
                        {
                            testMethods.Add(method);
                        }
                    }

                    testInfo.TestMethods = testMethods;
                    domainInfo.TestClasses.Add(type, testInfo);
                }
            }
            
        }

        private List<TestResult> Execute(DomainInfo domainInfo)
        {
            if (domainInfo == null || domainInfo.TestClasses == null || domainInfo.TestClasses.Count == 0)
            {
                return null;
            }

            List<TestResult> results = new List<TestResult>();

            foreach (KeyValuePair<Type, TestInfo> pair in domainInfo.TestClasses)
            {
                Type testClassType = pair.Key;
                TestResult result = new TestResult()
                {
                    TestClassName = testClassType.Name,
                    MethodTestInfoList = new List<MethodTestInfo>()
                };

                string assemblyLocation = testClassType.Assembly.Location;
                string directoryPath = assemblyLocation.Substring(0, assemblyLocation.LastIndexOf("\\"));
                Directory.SetCurrentDirectory(directoryPath);

                var testInstance = Activator.CreateInstance(testClassType);

                TestInfo testInfo = pair.Value;
                if (testInfo.InitMethod != null)
                {
                    MethodTestInfo methodTestInfo = RunMethod(testInstance, testInfo.InitMethod);
                    result.MethodTestInfoList.Add(methodTestInfo);
                }

                foreach (MethodInfo method in testInfo.TestMethods)
                {
                    MethodTestInfo methodTestInfo = RunMethod(testInstance, method);
                    result.MethodTestInfoList.Add(methodTestInfo);
                }

                results.Add(result);
            }

            return results;
        }

        private MethodTestInfo RunMethod(Object testInstance, MethodInfo method)
        {
            MethodTestInfo methodTestInfo = new MethodTestInfo()
            {
                ResultCode = TestResultCode.PASSED
            };

            Stopwatch stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();
                try
                {
                    method.Invoke(testInstance, null);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException != null)
                    {
                        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    }
                }
            }
            catch (AssertionException ex)
            {
                HandleException(ex, methodTestInfo);
            }
            catch (Exception ex)
            {
                HandleException(ex, methodTestInfo);
            }
            finally
            {
                stopWatch.Stop();
            }

            methodTestInfo.Duration = stopWatch.ElapsedMilliseconds;

            return methodTestInfo;
        }

        private void HandleException(Exception ex, MethodTestInfo methodTestInfo)
        {
            methodTestInfo.ResultCode = TestResultCode.FAILED;
            methodTestInfo.Exception = new ExceptionInfo()
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };
            if (ex.InnerException != null)
            {
                methodTestInfo.InnerException = new ExceptionInfo()
                {
                    Message = ex.InnerException.Message,
                    StackTrace = ex.InnerException.StackTrace
                };
            }
        }
    }
}
