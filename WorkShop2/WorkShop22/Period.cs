using System;

namespace WorkShop22
{
    public class Period
    {
        public Period(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime)
            {
                throw new ArgumentException();
            }

            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public bool IsSameMonth()
        {
            return StartTime.Month == EndTime.Month;
        }

        public int Days()
        {
            return EndTime.Subtract(StartTime).Days + 1;
        }

        public int OverlapDays(Period otherPeriod)
        {
            if (HasNoOverlap(otherPeriod))
            {
                return 0;
            }

            var overlapStart = StartTime > otherPeriod.StartTime
                ? StartTime
                : otherPeriod.StartTime;

            var overlapEnd = EndTime < otherPeriod.EndTime
                ? EndTime
                : otherPeriod.EndTime;

            return (overlapEnd.AddDays(1) - overlapStart).Days;
        }

        private bool HasNoOverlap(Period otherPeriod)
        {
            return otherPeriod.EndTime < StartTime || otherPeriod.StartTime > EndTime;
        }
    }
}