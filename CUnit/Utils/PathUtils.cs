using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUnit.Utils
{
    public static class PathUtils
    {
        public static string GetCurrentDirectory()
        {
            return AppDomain.CurrentDomain.GetData(Constants.AppDomainBasePath).ToString();
        }
    }
}
