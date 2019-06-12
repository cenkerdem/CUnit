# CUnit
CUnit is a unit testing framework for .NET projects. <br/>
It executes the registered projects on a central process. <br/>
CUnit.exe console application processes and shows the results.
# Usage
Projects that will be tested must add reference to CUnit library. <br/>
The classes that will be tested must have TestClass attribute. <br/>
If there is an initial preperation method, it will be executed firstly by adding Init attribute. <br/>
Test methods must have TestMethod attribute. <br/>
CUnit Assertion class is used for test validation.
```
[TestClass("Message Parser Test Class")]
    public class MessageParser
    {
        private string message = null;
        [InitMethod]
        public void Init()
        {
            string messageFilePath = Path.Combine(PathUtils.GetCurrentDirectory(), @"..\\..\\Test\Message.txt");
            message = File.ReadAllText(messageFilePath, Encoding.UTF8);
        }

        [TestMethod]
        public void TestGettingNumber() {
            SampleLibrary.MessageParser parser = new SampleLibrary.MessageParser();
            Message result = parser.Parse(message);
            Assert.Equals(result.Id, 2);
        }
```


Tested assembly information must be added to TestProjects.json file in CUnit root folder.
# Credits
Cenk Erdem
