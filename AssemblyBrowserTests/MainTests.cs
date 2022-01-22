using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AssemblyBrowserLibrary.Model;
using System;

namespace AssemblyBrowserTests
{
    [TestClass]
    public class MainTests
    {
        private List<NamespaceWrapper> AssemblyInfo;

        private readonly string PathToDLL = @"d:\Work\СПП\Часть 1\Laba2\FakerLib\bin\Debug\Faker.exe";

        [TestInitialize]
        public void Setup()
        {
            var assemblyBrowserInstance = new AssemblyBrowserLibrary.AssemblyBrowser();
            AssemblyInfo = assemblyBrowserInstance.GetAssemblyData(PathToDLL);
            
        }

        [TestMethod]
        public void TestNamespacesCount()
        {
            Assert.AreEqual(6, AssemblyInfo.Count);
        }

        [TestMethod]
        public void TestNamespacesNotEmpty()
        {
            foreach (var item in AssemblyInfo)
            {
                Assert.IsNotNull(item);
            }

        }

        [TestMethod]
        public void TestWithoutNamespacesTypes()
        {
            Assert.AreEqual(1, AssemblyInfo[0].Types.Count);

            Assert.IsNotNull(AssemblyInfo[0].Types[0]);

            Assert.IsTrue(AssemblyInfo[0].Types[0].Name.Equals("<>F{00000008}`5"));
        }

        [TestMethod]
        public void TestFakerLibTypes()
        {
            Assert.AreEqual(2, AssemblyInfo[1].Types.Count);
            
            Assert.IsNotNull(AssemblyInfo[1].Types[0]);
            Assert.IsNotNull(AssemblyInfo[1].Types[1]);

            Assert.IsTrue(AssemblyInfo[1].Types[0].Name.Equals("Program"));
            Assert.IsTrue(AssemblyInfo[1].Types[1].Name.Equals("<>c"));
        }

        [TestMethod]
        public void TestFakerLibTestTypes()
        {
            Assert.AreEqual(3, AssemblyInfo[2].Types.Count);

            Assert.IsNotNull(AssemblyInfo[2].Types[0]);
            Assert.IsNotNull(AssemblyInfo[2].Types[1]);
            Assert.IsNotNull(AssemblyInfo[2].Types[2]);

            Assert.IsTrue(AssemblyInfo[2].Types[0].Name.Equals("B"));
            Assert.IsTrue(AssemblyInfo[2].Types[1].Name.Equals("C"));
            Assert.IsTrue(AssemblyInfo[2].Types[2].Name.Equals("A"));
        }

        [TestMethod]
        public void TestFakerLibHelperTypes()
        {
            Assert.AreEqual(1, AssemblyInfo[3].Types.Count);

            Assert.IsNotNull(AssemblyInfo[3].Types[0]);

            Assert.IsTrue(AssemblyInfo[3].Types[0].Name.Equals("Loader"));
        }

        [TestMethod]
        public void TestFakerLibBlockTypes()
        {
            Assert.AreEqual(4, AssemblyInfo[4].Types.Count);

            Assert.IsNotNull(AssemblyInfo[4].Types[0]);
            Assert.IsNotNull(AssemblyInfo[4].Types[1]);
            Assert.IsNotNull(AssemblyInfo[4].Types[2]);
            Assert.IsNotNull(AssemblyInfo[4].Types[3]);

            Assert.IsTrue(AssemblyInfo[4].Types[0].Name.Equals("FakerConfig"));
            Assert.IsTrue(AssemblyInfo[4].Types[1].Name.Equals("Faker"));
            Assert.IsTrue(AssemblyInfo[4].Types[2].Name.Equals("<>o__20"));
            Assert.IsTrue(AssemblyInfo[4].Types[3].Name.Equals("<>o__21"));
        }

        [TestMethod]
        public void TestFakerLibApiTypes()
        {
            Assert.AreEqual(2, AssemblyInfo[5].Types.Count);

            Assert.IsNotNull(AssemblyInfo[5].Types[0]);
            Assert.IsNotNull(AssemblyInfo[5].Types[1]);

            Assert.IsTrue(AssemblyInfo[5].Types[0].Name.Equals("IFakerConfig"));
            Assert.IsTrue(AssemblyInfo[5].Types[1].Name.Equals("IFaker"));
        }
    }
}
