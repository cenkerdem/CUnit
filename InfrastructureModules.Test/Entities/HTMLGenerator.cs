using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModules.Test.Entities
{
    class HTMLGenerator
    {
        public string GetHTML(Customer customer)
        {
            return string.Format("<html><head></head><body>Customer Id = {0}, Name = {1}</body></html>", customer.Id, customer.Name);
        }
    }
}
