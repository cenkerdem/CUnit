using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleLibrary
{
    class MessageParser
    {
        public Message Parse(string message)
        {
            return new Message()
            {
                Id = 1,
                Name = "Cenk"
            };
        }
    }
}
