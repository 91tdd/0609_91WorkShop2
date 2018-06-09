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
            var total = 0m;
            if (period.StartTime.Month == period.EndTime.Month)
            {
                return CaluateBudget(period.StartTime, period.EndTime, budgets);
            }
            total += CaluateBudget(period.StartTime, endDayOfStartTimeMonth(period.StartTime), budgets);

            total += CaluateBudget(startDayOfEndTimeMonth(period.EndTime), period.EndTime, budgets);

            DateTime Counter = period.StartTime;
            if (IsOver2Months(period.StartTime, period.EndTime))
            {
                do
                {
                    total += CaluateBudget(Counter.AddMonths(1), Counter.AddMonths(2).AddDays(-1), budgets);
                    Counter = Counter.AddMonths(1);
                } while (Counter.Month != period.EndTime.AddMonths(-1).Month);
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

        private static int CaluateBudget(DateTime startTime, DateTime endTime, List<Budget> budgets)
        {
            return (endTime.Subtract(startTime).Days + 1) * GetThisMonthBudget(startTime, budgets) / DateTime.DaysInMonth(startTime.Year, startTime.Month);
        }

        private static int GetThisMonthBudget(DateTime startTime, List<Budget> budgets)
        {
            return budgets.SingleOrDefault(x => x.YearMonth == startTime.ToString("yyyyMM"))?.Amount ?? 0;
        }
    }
}