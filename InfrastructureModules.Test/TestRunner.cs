using InfrastructureModules.Test.Attributes;
using InfrastructureModules.Test.Entities;
using InfrastructureModules.Test.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test
{
    class TestRunner
    {
        private Dictionary<Type, TestInfo> testClasses;

        public TestRunner()
        {
            testClasses = new Dictionary<Type, TestInfo>();
        }

        public List<TestResult> Run()
        {
            LoadAssemblies();
            AnalyzeTestMethods();
            return Execute();
        }

        private void LoadAssemblies()
        {

        }

        private void AnalyzeTestMethods()
        {
            Type[] types = this.GetType().Assembly.GetTypes();
            foreach (Type type in types)
            {
                Attribute attribute = type.GetCustomAttribute(typeof(TestClass));
                if (attribute == null)
                {
                    continue;
                }

                TestInfo testInfo = new TestInfo()
                {
                    ClassType = type
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
                testClasses.Add(type, testInfo);
            }
        }

        private List<TestResult> Execute()
        {
            if (testClasses == null || testClasses.Count == 0)
            {
                return null;
            }

            List<TestResult> results = new List<TestResult>();

            foreach(KeyValuePair<Type, TestInfo> pair in testClasses) {
                TestResult result = new TestResult()
                {
                    TestClass = pair.Key,
                    MethodTestInfoList = new List<MethodTestInfo>()
                };

                var testInstance = Activator.CreateInstance(pair.Key);

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
                ResultCode = TestResultCode.SUCCESS
            };

            try
            {
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

            return methodTestInfo;
        }

        private void HandleException(Exception ex, MethodTestInfo methodTestInfo)
        {
            methodTestInfo.ResultCode = TestResultCode.ERROR;
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
