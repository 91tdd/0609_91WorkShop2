using System;
using System.Collections.Generic;
using System.Linq;
using WorkShop2.Tests;

namespace WorkShop22
{
    public class BudgetCalculator
    {
        private readonly IRepository<Budget> _budRepository;

        public BudgetCalculator(IRepository<Budget> budRepository)
        {
            _budRepository = budRepository;
        }

        internal decimal TotalAmount(DateTime startTime, DateTime endTime)
        {
            var period = new Period(startTime, endTime);
            var budgets = _budRepository.GetBudgets();
            var budget = budgets.SingleOrDefault(x => x.YearMonth == period.StartTime.ToString("yyyyMM"));

            if (period.IsSameMonth())
            {
                if (budget == null)
                {
                    return 0;
                }

                return period.Days() * budget.DailyAmount();
            }

            var total = TotalAmountWhenPeriodOverlapMultiMonths(period, budgets);
            return total;
        }

        private static decimal TotalAmountWhenPeriodOverlapMultiMonths(Period period, List<Budget> budgets)
        {
            var total = 0m;
            var currentMonth = period.StartTime;
            while (currentMonth <= period.EndTime.AddMonths(1))
            {
                var budget = budgets.SingleOrDefault(x => x.YearMonth == currentMonth.ToString("yyyyMM"));
                if (budget != null)
                {
                    var amountOfCurrentMonth = OverlappingDays(period, budget) * budget.DailyAmount();
                    total += amountOfCurrentMonth;
                }
                currentMonth = currentMonth.AddMonths(1);
            }

            return total;
        }

        private static int OverlappingDays(Period period, Budget budget)
        {
            var overlapStartDate = period.StartTime > budget.FirstDay
                ? period.StartTime
                : budget.FirstDay;

            var overlapEndDate = period.EndTime < budget.LastDay
                ? period.EndTime
                : budget.LastDay;

            var overlappingDays = new Period(overlapStartDate, overlapEndDate).Days();
            return overlappingDays;
        }

        private static bool IsLastMonthOfPeriod(Period period, DateTime currentMonth)
        {
            return currentMonth.ToString("yyyyMM") == period.EndTime.ToString("yyyyMM");
        }

        private static bool IsFirstMonthOfPeriod(Period period, DateTime currentMonth)
        {
            return currentMonth.ToString("yyyyMM") == period.StartTime.ToString("yyyyMM");
        }

        private static DateTime GetFirstDay(DateTime currentDate)
        {
            return new DateTime(currentDate.Year, currentDate.Month, 1);
        }

        private static DateTime GetLastDay(DateTime currentDate)
        {
            return new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1).AddDays(-1);
        }

        private static bool IsOver2Months(DateTime startTime, DateTime endTime)
        {
            var startTime1 = new DateTime(startTime.Year, startTime.Month, 1);
            var endTime1 = new DateTime(endTime.Year, endTime.Month, 1);
            return startTime1.AddMonths(2) < endTime1;
        }
    }
}