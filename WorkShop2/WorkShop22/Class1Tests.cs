using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
namespace WorkShop2.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        [TestMethod()]
        public void ResultTest()
        {
            IRepository<Budget> repo = Substitute.For<IRepository<Budget>>();
            repo.GetBudgets().Returns(new List<Budget>()
            {
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201806", Amount = 300 },
                new Budget() { YearMonth = "201807", Amount = 310 }

            });
            var target = new WorkShop22.BudgetCalculate(repo);

            var actual = target.Result(new DateTime(2018,6,1),new DateTime(2018,6,30));
            Assert.AreEqual(300m, actual);
        }
    }
}