using System;

namespace WorkShop22
{
    public class Period
    {
        public Period(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            if (StartTime > EndTime)
            {
                throw new ArgumentException();
            }
        }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public bool IsSameMonth()
        {
            return StartTime.Month == EndTime.Month;
        }
    }
}