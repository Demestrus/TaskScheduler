using System;
using System.Collections.Generic;
using System.Linq;

namespace Application
{
    public class Activity
    {
        private readonly HashSet<Activity> _dependsOn;
        
        public Activity(long id)
        {
            Id = id;
            _dependsOn = new HashSet<Activity>();
        }

        public Activity(long id, IEnumerable<Activity> dependingActivities)
        {
            Id = id;
            _dependsOn = dependingActivities.ToHashSet();
        }

        public long Id { get; }

        public bool IsIndependent => _dependsOn.Count == 0;
        
        public bool IsDependingOn(IEnumerable<Activity> activities)
        {
            return _dependsOn.IsSubsetOf(activities);
        }
    }
}