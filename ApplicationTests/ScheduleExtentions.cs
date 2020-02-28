using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Application;

namespace ApplicationTests
{
    public static class ScheduleExtensions
    {
        public static bool IsEqualTo(this Schedule schedule, Schedule anotherSchedule)
        {
            if (schedule.Activities.Count != anotherSchedule.Activities.Count) return false;

            for (int i = 0; i < schedule.Activities.Count; i++)
            {
                if (schedule.Activities[i].Id != anotherSchedule.Activities[i].Id) return false;
            }

            return true;
        }
    }
    
    public class ScheduleComparer : IComparer<Schedule>
    {
        public int Compare(Schedule x, Schedule y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentNullException();
            }

            var xFinish = x.GetFinishTime();
            var yFinish = y.GetFinishTime();
            
            if (xFinish > yFinish) return 1;

            if (yFinish > xFinish) return -1;

            if (x.DurableActivities.Count > y.DurableActivities.Count) return 1;

            if (y.DurableActivities.Count > x.DurableActivities.Count) return -1;

            for (int i = 0; i < x.DurableActivities.Count; i++)
            {
                if (x.DurableActivities[i].StartTime > y.DurableActivities[i].StartTime) return 1;
                if (y.DurableActivities[i].StartTime > x.DurableActivities[i].StartTime) return -1;
            }
            
            for (int i = 0; i < x.DurableActivities.Count; i++)
            {
                if (x.DurableActivities[i].Id > y.DurableActivities[i].Id) return 1;
                if (y.DurableActivities[i].Id > x.DurableActivities[i].Id) return -1;
            }

            return 0;
        }
    }
}