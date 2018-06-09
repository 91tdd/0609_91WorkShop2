using System;
using System.Collections.Generic;
using System.Linq;
using WorkShop2.Tests;

namespace WorkShop22
{
    public class BudgetCalculate
    {
        private readonly IRepository<Budget> _budRepository;

        public BudgetCalculate(IRepository<Budget> budRepository)
        {
            _budRepository = budRepository;
        }

        internal decimal Result(DateTime startTime, DateTime endTime)
        {
            var budgets = _budRepository.GetBudgets();
            var total = 0m;
            if (startTime.Month == endTime.Month)
            {
                var hasBudget = budgets.Exists(x => startTime.ToString("yyyyMM") == x.YearMonth ||
                                                    endTime.ToString("yyyyMM") == x.YearMonth);
                if (!hasBudget)
                {
                    return 0m;
                }
                return CaluateBudget(startTime, endTime, budgets);
            }
            total += CaluateBudget(startTime, new DateTime(startTime.Year, startTime.Month, 1).AddMonths(1).AddDays(-1), budgets);

            total += CaluateBudget(new DateTime(endTime.Year, endTime.Month, 1), endTime, budgets);

            DateTime Counter = startTime;
            if (endTime.Month - startTime.Month >= 2)
            {
                do
                {
                    total += CaluateBudget(Counter.AddMonths(1), Counter.AddMonths(2).AddDays(-1), budgets);
                    Counter = Counter.AddMonths(1);
                } while (Counter.Month != endTime.Month);
            }
            return total;
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