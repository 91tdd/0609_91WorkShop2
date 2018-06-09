using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkShop2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkShop2.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        [TestMethod()]
        public void ResultTest()
        {
            var class1 = new Class1();
            class1.Result();
            Assert.Fail();
        }
    }
}