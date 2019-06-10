# CUnit
CUnit is a custom unit testing framework, executes the registered projects on a central process.
CUnit.exe console application processes and shows the results.
# Usage
Libraries that will be tested must add reference to CUnit dll.
The classes that will be tested must have TestClass attribute.
If there is an initial preperation method, it will be executed firstly by adding Init attribute.
Test methods must have TestMethod attribute.
CUnit Assertion class is used for test validation.

Tested assembly information must be added to TestProjects.json file in CUnit root folder.
