using System;
using System.Collections.Generic;

namespace Application
{
    public class DurableActivity : Activity
    {
        public DurableActivity(long id, DateTime startTime, TimeSpan duration) : base(id)
        {
            StartTime = startTime;
            Duration = duration;
        }

        public DurableActivity(long id, IEnumerable<Activity> dependingActivities, DateTime startTime, TimeSpan duration) : base(id, dependingActivities)
        {
            StartTime = startTime;
            Duration = duration;
        }
        
        public DateTime StartTime { get; }
        public TimeSpan Duration { get; }

        public DateTime EndTime => StartTime.Add(Duration);
    }
}