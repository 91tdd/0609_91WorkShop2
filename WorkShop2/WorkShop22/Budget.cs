using System;
using WorkShop22;

namespace WorkShop2.Tests
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public DateTime FirstDay
        {
            get { return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null); }
        }

        public DateTime LastDay
        {
            get { return DateTime.ParseExact(YearMonth + DaysInMonth(), "yyyyMMdd", null); }
        }

        private int DaysInMonth()
        {
            return DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
        }

        public int DailyAmount()
        {
            return Amount / DaysInMonth();
        }

        public Period GetPeriod()
        {
            return new Period(FirstDay, LastDay);
        }

        public int EffectiveAmount(Period period)
        {
            return period.OverlapDays(GetPeriod()) * DailyAmount();
        }
    }
}