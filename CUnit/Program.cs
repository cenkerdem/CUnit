using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUnit.Entities;
using System.IO;

namespace CUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            TestManager testManager = new TestManager();
            Dictionary<string, List<ClassTestResult>> projectTestResults = testManager.Run();
            string whiteSpace = " ";
            bool isStackTraceIncluded = false;
            foreach (var projectTestInfo in projectTestResults)
            {
                Console.WriteLine(string.Format("Assembly Name = \"{0}\"", projectTestInfo.Key));

                List<ClassTestResult> testResults = projectTestInfo.Value;
                foreach (ClassTestResult testResult in testResults)
                {
                    Console.WriteLine(string.Format("{0}Class Name = \"{1}\"", whiteSpace.PadRight(2), testResult.TestClassName));
                    foreach (var methodTestInfo in testResult.MethodTestInfoList)
                    {
                        Console.WriteLine(string.Format("{0}Method Name = \"{1}\" -> Result = {2}, Duration = {3}", whiteSpace.PadRight(4), methodTestInfo.MethodName, methodTestInfo.ResultCode.ToString(), methodTestInfo.Duration));
                        if (methodTestInfo.ResultCode == TestResultCode.FAILED && methodTestInfo.Exception != null)
                        {
                            Console.WriteLine(string.Format("{0}Exception Message = \"{1}\"", whiteSpace.PadRight(6), methodTestInfo.Exception.Message));
                            if (isStackTraceIncluded)
                            {
                                Console.WriteLine(string.Format("{0}Exception Stack Trace = \"{1}\"", whiteSpace.PadRight(6), methodTestInfo.Exception.StackTrace));
                            }
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
