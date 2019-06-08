using InfrastructureModules.Test.Attributes;
using InfrastructureModules.Test.Entities;
using InfrastructureModules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test
{
    [TestClass("SingletonProvider test class")]
    class SingletonProvider
    {
        private Customer customer;

        [InitMethod]
        public void Init()
        {
            customer = new Customer()
            {
                Id = 1,
                Name = "Cenk"
            };
        }

        [TestMethod]
        public void Test()
        {
            HTMLGenerator generator = SingletonProvider<HTMLGenerator>.Instance;
            string generatedHTML = generator.GetHTML(customer);
            Assert.Equals(1, 2);
        }
    }
}
