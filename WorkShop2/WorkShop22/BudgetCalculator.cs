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
                    total += budget.OverlappingAmount(period);
                }
                currentMonth = currentMonth.AddMonths(1);
            }

            return total;
        }
    }
}