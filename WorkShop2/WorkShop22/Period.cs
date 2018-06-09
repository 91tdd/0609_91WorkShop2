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

        public int Days()
        {
            var days = EndTime.Subtract(StartTime).Days + 1;
            return days;
        }

        public int OverlappingDays(Period otherPeriod)
        {
            var overlapStartDate = StartTime > otherPeriod.StartTime
                ? StartTime
                : otherPeriod.StartTime;

            var overlapEndDate = EndTime < otherPeriod.EndTime
                ? EndTime
                : otherPeriod.EndTime;

            var overlappingDays = new Period(overlapStartDate, overlapEndDate).Days();
            return overlappingDays;
        }
    }
}