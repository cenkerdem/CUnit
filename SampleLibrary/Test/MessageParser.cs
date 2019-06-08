﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureModules.Test.Attributes;
using System.IO;
using InfrastructureModules.Test;

namespace SampleLibrary.Test
{
    [TestClass("Message Parser Test Class")]
    public class MessageParser
    {
        private string message = null;
        [InitMethod]
        public void Init()
        {
            message = File.ReadAllText(string.Format("{0}\\Message.txt", AppDomain.CurrentDomain.BaseDirectory), Encoding.UTF8);
        }

        [TestMethod]
        public void TestGettingNumber() {
            SampleLibrary.MessageParser parser = new SampleLibrary.MessageParser();
            Message result = parser.Parse(message);
            Assert.Equals(result.Id, 1);
        }
    }
}