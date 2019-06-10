using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureModules.Test.Attributes;
using System.IO;
using InfrastructureModules.Test;
using InfrastructureModules.Test.Utils;
using System.Threading;

namespace SampleLibrary.Test
{
    [TestClass("Message Parser Test Class")]
    public class MessageParser
    {
        private string message = null;
        [InitMethod]
        public void Init()
        {
            //string messageFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\\..\\Test\Message.txt");
            string messageFilePath = Path.Combine(PathUtils.GetCurrentDirectory(), @"..\\..\\Test\Message.txt");
            message = File.ReadAllText(messageFilePath, Encoding.UTF8);
        }

        [TestMethod]
        public void TestGettingNumber() {
            SampleLibrary.MessageParser parser = new SampleLibrary.MessageParser();
            Message result = parser.Parse(message);
            Thread.Sleep(500);
            Assert.Equals(result.Id, 2);
        }
    }
}
