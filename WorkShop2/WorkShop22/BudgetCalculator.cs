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

            var total = 0m;
            if (period.IsSameMonth())
            {
                return TotalAmountWhenPeriodIsSameMonth(budget, period);
            }

            total += CalculateBudget(period.StartTime, EndDayOfStartTimeMonth(period.StartTime), budgets);

            total += CalculateBudget(StartDayOfEndTimeMonth(period.EndTime), period.EndTime, budgets);

            DateTime Counter = period.StartTime;
            if (IsOver2Months(period.StartTime, period.EndTime))
            {
                do
                {
                    total += CalculateBudget(Counter.AddMonths(1), Counter.AddMonths(2).AddDays(-1), budgets);
                    Counter = Counter.AddMonths(1);
                } while (Counter.Month != period.EndTime.AddMonths(-1).Month);
            }
            return total;
        }

        private static decimal TotalAmountWhenPeriodIsSameMonth(Budget budget, Period period)
        {
            if (budget == null)
            {
                return 0;
            }

            return period.Days() * budget.DailyAmount();
        }

        private static DateTime StartDayOfEndTimeMonth(DateTime endTime)
        {
            return new DateTime(endTime.Year, endTime.Month, 1);
        }

        private static DateTime EndDayOfStartTimeMonth(DateTime startTime)
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

            var days = endTime.Subtract(startTime).Days + 1;
            return days * budget.DailyAmount();
        }
    }
}