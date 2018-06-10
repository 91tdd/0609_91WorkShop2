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
                    var effectiveAmount = EffectiveAmount(period, budget);
                    total += effectiveAmount;
                }

                currentMonth = currentMonth.AddMonths(1);
            }

            return total;
        }

        private static int EffectiveAmount(Period period, Budget budget)
        {
            return period.OverlapDays(new Period(budget.FirstDay, budget.LastDay)) * budget.DailyAmount();
        }
    }
}