using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (startTime.Month != endTime.Month)
            {
                var total = 0m;
                total += CaluateBudget(startTime, new DateTime(startTime.Year, startTime.Month + 1, 1).AddDays(-1), budgets);
                total += CaluateBudget(new DateTime(endTime.Year, endTime.Month, 1), endTime, budgets);
                return total;
            }
            if (startTime.Month == endTime.Month)
            {
                return CaluateBudget(startTime, endTime, budgets);
            }
            var hasBudget = budgets.Exists(x => startTime.ToString("yyyyMM") == x.YearMonth ||
            endTime.ToString("yyyyMM") == x.YearMonth);
            if (!hasBudget)
            {
                return 0m;
            }
            return 300m;
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
