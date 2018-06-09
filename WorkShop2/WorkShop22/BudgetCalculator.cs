using System;
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

            return _budRepository.GetBudgets()
                .Sum(b => b.OverlappingAmount(period));
        }
    }
}