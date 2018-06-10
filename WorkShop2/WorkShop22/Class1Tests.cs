using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;

namespace WorkShop2.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        private IRepository<Budget> _budgetRepository = Substitute.For<IRepository<Budget>>();
        private WorkShop22.BudgetCalculator _budgetCalculator;

        [TestInitialize]
        public void TestInit()
        {
            //GivenBudgets();
            _budgetCalculator = new WorkShop22.BudgetCalculator(_budgetRepository);
        }

        [TestMethod()]
        public void OneMonthFullBudget()
        {
            GivenBudgets(new Budget() { YearMonth = "201806", Amount = 300 });
            BudgetResultShouldBe(new DateTime(2018, 6, 1), new DateTime(2018, 6, 30), 300m);
        }

        [TestMethod()]
        public void OneMonthPartialBudget()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201806", Amount = 300 }
            );
            BudgetResultShouldBe(new DateTime(2018, 6, 1), new DateTime(2018, 6, 15), 150m);
        }

        [TestMethod()]
        public void TwoMonthBothNotBudget()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201806", Amount = 300 }
            );
            BudgetResultShouldBe(new DateTime(2018, 3, 1), new DateTime(2018, 4, 2), 0m);
        }

        [TestMethod()]
        public void TwoMonth_StartMonthNoBudget_EndMonthHasBudget()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201806", Amount = 300 }
            );
            BudgetResultShouldBe(new DateTime(2018, 5, 20), new DateTime(2018, 6, 10), 100m);
        }

        [TestMethod()]
        public void TwoMonth_StartMonthHasBudget_EndMonthNoBudget()
        {
            GivenBudgets(new Budget() { YearMonth = "201807", Amount = 310 });
            BudgetResultShouldBe(new DateTime(2018, 7, 30), new DateTime(2018, 8, 22), 20m);
        }

        [TestMethod()]
        public void TwoMonthBothBudget()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201806", Amount = 300 },
                new Budget() { YearMonth = "201807", Amount = 310 }
            );
            BudgetResultShouldBe(new DateTime(2018, 6, 15), new DateTime(2018, 7, 14), 300m);
        }

        [TestMethod()]
        public void OverYearBudget()
        {
            GivenBudgets(new Budget() { YearMonth = "201802", Amount = 280 });
            BudgetResultShouldBe(new DateTime(2017, 12, 1), new DateTime(2018, 2, 1), 10m);
        }

        [TestMethod()]
        public void OverYearBudget_EndYearHas2Budget()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201806", Amount = 300 }
            );
            BudgetResultShouldBe(new DateTime(2017, 11, 1), new DateTime(2018, 6, 30), 580m);
        }

        [TestMethod()]
        public void AllYearBudget()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201806", Amount = 300 },
                new Budget() { YearMonth = "201807", Amount = 310 }
            );
            BudgetResultShouldBe(new DateTime(2018, 01, 1), new DateTime(2018, 12, 31), 890);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod()]
        public void ThrowExpection()
        {
            var result = _budgetCalculator.Result(new DateTime(2018, 5, 1), new DateTime(2018, 4, 30));
        }

        private void BudgetResultShouldBe(DateTime startTime, DateTime endTime, decimal expected)
        {
            var actual = _budgetCalculator.Result(startTime, endTime);
            Assert.AreEqual(expected, actual);
        }

        private void GivenBudgets(params Budget[] budgets)
        {
            _budgetRepository.GetBudgets().Returns(budgets.ToList());
        }
    }
}