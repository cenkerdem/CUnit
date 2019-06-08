using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test.Entities
{
    class TestResult
    {
        public Type TestClass { get; set; }
        public int PassedCount { get; set; }
        public int FailedCount { get; set; }
        public List<MethodTestInfo> MethodTestInfoList { get; set; }
    }

    class MethodTestInfo
    {
        public TestResultCode ResultCode { get; set; }
        public ExceptionInfo Exception { get; set; }
        public ExceptionInfo InnerException { get; set; }
    }

    class ExceptionInfo
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    enum TestResultCode
    {
        SUCCESS = 1,
        ERROR = 2
    }
}
