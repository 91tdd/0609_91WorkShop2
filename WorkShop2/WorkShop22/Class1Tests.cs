using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace WorkShop2.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        private IRepository<Budget> _budRepository = Substitute.For<IRepository<Budget>>();
        private WorkShop22.BudgetCalculate _budgetCalculate;

        [TestInitialize]
        public void TestInit()
        {
            _budgetCalculate = GiveBudgets();
        }

        [TestMethod()]
        public void OneMonthFullBudget()
        {
            BudgetResultShouldBe(new DateTime(2018, 6, 1), new DateTime(2018, 6, 30), 300m);
        }

        [TestMethod()]
        public void OneMonthPartialBudget()
        {
            BudgetResultShouldBe(new DateTime(2018, 6, 1), new DateTime(2018, 6, 15), 150m);
        }
        [TestMethod()]
        public void Test09()
        {
            BudgetResultShouldBe(new DateTime(2018, 3, 1), new DateTime(2018, 4, 2), 0m);
        }

        private void BudgetResultShouldBe(DateTime startTime, DateTime endTime, decimal expected)
        {
            var actual = _budgetCalculate.Result(startTime, endTime);
            Assert.AreEqual(expected, actual);
        }

        private WorkShop22.BudgetCalculate GiveBudgets()
        {
            _budRepository.GetBudgets().Returns(new List<Budget>()
            {
                new Budget() {YearMonth = "201802", Amount = 280},
                new Budget() {YearMonth = "201806", Amount = 300},
                new Budget() {YearMonth = "201807", Amount = 310}
            });
            var target = new WorkShop22.BudgetCalculate(_budRepository);
            return target;
        }
    }
}