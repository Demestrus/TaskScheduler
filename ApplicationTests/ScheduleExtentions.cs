using System.Collections;
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
}