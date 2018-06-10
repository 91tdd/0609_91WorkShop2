﻿using System;
using System.Collections.Generic;
using System.Linq;
using WorkShop2.Tests;

namespace WorkShop22
{
    public class Period
    {
        public Period(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public bool IsInvalid()
        {
            return StartTime > EndTime;
        }
    }

    public class BudgetCalculator
    {
        private readonly IRepository<Budget> _budgetRepository;

        public BudgetCalculator(IRepository<Budget> budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        internal decimal Result(DateTime startTime, DateTime endTime)
        {
            var period = new Period(startTime, endTime);
            if (period.IsInvalid())
            {
                throw new ArgumentException();
            }
            var budgets = _budgetRepository.GetBudgets();
            var total = 0m;
            if (startTime.Month == endTime.Month)
            {
                return CaluateBudget(startTime, endTime, budgets);
            }
            total += CaluateBudget(startTime, endDayOfStartTimeMonth(startTime), budgets);

            total += CaluateBudget(startDayOfEndTimeMonth(endTime), endTime, budgets);

            DateTime Counter = startTime;
            if (IsOver2Months(startTime, endTime))
            {
                do
                {
                    total += CaluateBudget(Counter.AddMonths(1), Counter.AddMonths(2).AddDays(-1), budgets);
                    Counter = Counter.AddMonths(1);
                } while (Counter.Month != endTime.AddMonths(-1).Month);
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
            return endTime.Month - startTime.Month >= 2;
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