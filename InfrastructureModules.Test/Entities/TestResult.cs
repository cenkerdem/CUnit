using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test.Entities
{
    [Serializable]
    class TestResult
    {
        public string TestClassName { get; set; }
        public int PassedCount { get; set; }
        public int FailedCount { get; set; }
        public List<MethodTestInfo> MethodTestInfoList { get; set; }
    }

    [Serializable]
    class MethodTestInfo
    {
        public TestResultCode ResultCode { get; set; }
        public ExceptionInfo Exception { get; set; }
        public ExceptionInfo InnerException { get; set; }
        public long Duration { get; set; }
    }

    [Serializable]
    class ExceptionInfo
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    enum TestResultCode
    {
        PASSED = 1,
        FAILED = 2
    }
}
