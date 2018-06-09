using System;

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
    }
}