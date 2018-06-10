using System;
using WorkShop22;

namespace WorkShop2.Tests
{
    public class Budget
    {
        public int Amount { get; set; }

        public DateTime FirstDay
        {
            get { return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null); }
        }

        public DateTime LastDay
        {
            get { return DateTime.ParseExact(YearMonth + DaysInMonth, "yyyyMMdd", null); }
        }

        public string YearMonth { get; set; }

        private int DailyAmount
        {
            get
            {
                return Amount / DaysInMonth;
            }
        }

        private int DaysInMonth
        {
            get
            {
                return DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            }
        }

        public int EffectiveAmount(Period period)
        {
            return period.OverlapDays(GetPeriod()) * DailyAmount;
        }

        private Period GetPeriod()
        {
            return new Period(FirstDay, LastDay);
        }
    }
}