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
    class TestRunner : MarshalByRefObject
    {
        public TestRunner()
        {
        }

        public List<ClassTestResult> Run(DomainInfo domainInfo)
        {
            AnalyzeTestMethods(domainInfo);
            return Execute(domainInfo);
        }

        private void AnalyzeTestMethods(DomainInfo domainInfo)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            domainInfo.TestClasses = new Dictionary<Type, TestInfo>();
            foreach (Assembly assembly in assemblies)
            {
                string assemblyName = assembly.GetName().Name;
                bool isTestAssembly = false;
                foreach (AssemblyInfo testAssembly in domainInfo.Assemblies)
                {
                    string testAssemblyName = testAssembly.AssemblyName;
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

        private List<ClassTestResult> Execute(DomainInfo domainInfo)
        {
            if (domainInfo == null || domainInfo.TestClasses == null || domainInfo.TestClasses.Count == 0)
            {
                return null;
            }

            List<ClassTestResult> results = new List<ClassTestResult>();

            foreach (KeyValuePair<Type, TestInfo> pair in domainInfo.TestClasses)
            {
                Type testClassType = pair.Key;
                ClassTestResult result = new ClassTestResult()
                {
                    TestClassName = testClassType.Name,
                    MethodTestInfoList = new List<MethodTestInfo>()
                };

                string assemblyLocation = testClassType.Assembly.Location;
                string directoryPath = assemblyLocation.Substring(0, assemblyLocation.LastIndexOf("\\"));
                domainInfo.AppDomain.SetData(Constants.AppDomainBasePath, directoryPath);
                //Directory.SetCurrentDirectory(directoryPath);

                var testInstance = Activator.CreateInstance(testClassType);

                TestInfo testInfo = pair.Value;
                if (testInfo.InitMethod != null)
                {
                    MethodTestInfo methodTestInfo = RunMethod(testInstance, testInfo.InitMethod);
                    result.MethodTestInfoList.Add(methodTestInfo);
                    if (methodTestInfo.ResultCode == TestResultCode.FAILED)
                    {
                        //If init method fails, initial state may not be ready for test methods.
                        continue;
                    }
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
                MethodName = method.Name,
                MethodNamespace = method.Module.FullyQualifiedName,
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
