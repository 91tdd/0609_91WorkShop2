using System;
using System.Collections.Generic;
using System.Linq;
using WorkShop2.Tests;

namespace WorkShop22
{
    public class BudgetCalculator
    {
        private readonly IRepository<Budget> _budgetRepository;

        public BudgetCalculator(IRepository<Budget> budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        internal decimal TotalAmount(DateTime startTime, DateTime endTime)
        {
            var period = new Period(startTime, endTime);
            var budgets = _budgetRepository.GetBudgets();
            if (period.IsSameMonth())
            {
                var budget = budgets.SingleOrDefault(x => x.YearMonth == period.StartTime.ToString("yyyyMM"));
                if (budget == null)
                {
                    return 0;
                }

                return period.Days() * budget.DailyAmount();
            }

            return TotalAmountWhenMultiMonthsPeriod(period, budgets);
        }

        private static decimal TotalAmountWhenMultiMonthsPeriod(Period period, List<Budget> budgets)
        {
            var total = 0m;
            DateTime currentMonth = period.StartTime;
            while (currentMonth <= period.EndTime.AddMonths(1))
            {
                var budget = budgets.SingleOrDefault(x => x.YearMonth == currentMonth.ToString("yyyyMM"));

                if (budget != null)
                {
                    var overlapStart = period.StartTime.ToString("yyyyMM") == currentMonth.ToString("yyyyMM")
                        ? period.StartTime
                        : budget.FirstDay;

                    var overlapEnd = period.EndTime.ToString("yyyyMM") == currentMonth.ToString("yyyyMM")
                        ? period.EndTime
                        : budget.LastDay;

                    var effectiveAmount = CalculateBudget(overlapStart, overlapEnd, budgets);
                    total += effectiveAmount;
                }

                currentMonth = currentMonth.AddMonths(1);
            }

            return total;
        }

        private static DateTime startDayOfEndTimeMonth(DateTime endTime)
        {
            return new DateTime(endTime.Year, endTime.Month, 1);
        }

        private static DateTime endDayOfStartTimeMonth(DateTime startTime)
        {
            return new DateTime(startTime.Year, startTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        private static bool IsOver2Months(DateTime startTime, DateTime endTime)
        {
            var startTime1 = new DateTime(startTime.Year, startTime.Month, 1);
            var endTime1 = new DateTime(endTime.Year, endTime.Month, 1);
            return startTime1.AddMonths(2) < endTime1;
        }

        private static int CalculateBudget(DateTime startTime, DateTime endTime, List<Budget> budgets)
        {
            var budget = budgets.SingleOrDefault(x => x.YearMonth == startTime.ToString("yyyyMM"));
            if (budget == null)
            {
                return 0;
            }

            return new Period(startTime, endTime).Days() * budget.DailyAmount();
        }
    }
}