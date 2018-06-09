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

        public DateTime EndTime { get; private set; }
        public DateTime StartTime { get; private set; }

        public int OverlappingDays(Period otherPeriod)
        {
            if (HasNoOverlap(otherPeriod))
            {
                return 0;
            }

            var overlapStartDate = StartTime > otherPeriod.StartTime
                ? StartTime
                : otherPeriod.StartTime;

            var overlapEndDate = EndTime < otherPeriod.EndTime
                ? EndTime
                : otherPeriod.EndTime;

            return (overlapEndDate - overlapStartDate).Days + 1;
        }

        private bool HasNoOverlap(Period otherPeriod)
        {
            return otherPeriod.EndTime < StartTime || otherPeriod.StartTime > EndTime;
        }
    }
}