using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Application
{
    public class Schedule
    {
        private readonly List<Activity> _activities;

        public Schedule()
        {
            _activities = new List<Activity>();
        }
        
        public Schedule(IEnumerable<Activity> activities)
        {
            _activities = activities.ToList();
        }

        public ImmutableList<Activity> Activities => _activities.ToImmutableList();

        public ImmutableList<DurableActivity> DurableActivities =>
            _activities.OfType<DurableActivity>().ToImmutableList();
        
        public void AddActivity(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));
            
            _activities.Add(activity);
        }

        public Schedule Copy()
        {
            return new Schedule(_activities);
        }

        public DateTime GetFinishTime()
        {
            var endTime = DateTime.MinValue;

            foreach (var activity in DurableActivities)
            {
                endTime = activity.StartTime > endTime ? activity.EndTime : endTime.Add(activity.Duration);
            }

            return endTime;
        }
    }
}