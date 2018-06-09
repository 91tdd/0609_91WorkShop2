using System;
using WorkShop22;

namespace WorkShop2.Tests
{
    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }

        private int DaysInMonth
        {
            get
            {
                return DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            }
        }

        private DateTime FirstDay
        {
            get
            {
                return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
            }
        }

        private DateTime LastDay
        {
            get { return DateTime.ParseExact(YearMonth + DaysInMonth, "yyyyMMdd", null); }
        }

        public int OverlappingAmount(Period period)
        {
            return period.OverlappingDays(PeriodFromBudget()) * DailyAmount();
        }

        private int DailyAmount()
        {
            return Amount / DaysInMonth;
        }

        private Period PeriodFromBudget()
        {
            return new Period(FirstDay, LastDay);
        }
    }
}