using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CUnit
{
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
}
