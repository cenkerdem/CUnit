using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureModules.Utils;
using InfrastructureModules.Test.Entities;

namespace InfrastructureModules.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRunner testRunner = new TestRunner();
            testRunner.Run();
            return;

            bool loopContinues = true;
            while (loopContinues)
            {
                string functionCode = Console.ReadLine();
                
                switch (functionCode.ToUpperInvariant())
                {
                    case "SINGLETON":
                        TestSingleton();
                        break;
                    case "\0":
                        loopContinues = false;
                        break;
                }
            }
        }

        private static void TestSingleton()
        {
            HTMLGenerator generator = SingletonProvider<HTMLGenerator>.Instance;
            Customer testCustomer = new Customer()
            {
                Id = 1,
                Name = "Cenk"
            };
            string generatedHTML = generator.GetHTML(testCustomer);
        }
    }
}
