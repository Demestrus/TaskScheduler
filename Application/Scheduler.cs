using System.Collections.Generic;
using System.Linq;

namespace Application
{
    public class Scheduler
    {
        private readonly Activity[] _activities;

        public Scheduler(Activity[] activities)
        {
            _activities = activities;
        }

        private IEnumerable<DurableActivity> DurableActivities => _activities.OfType<DurableActivity>();
        
        public List<Schedule> GetSchedules()
        {
            return CalculateSchedules(new Schedule());
        }

        public Schedule FindShortestSchedule()
        {
            var schedule = new Schedule();

            DurableActivity nextBetterActivity;

            // чтобы не перебирать все возможные варианты из CalculateSchedules
            // на каждом шаге выбора следующей activity принимаем, что наиболее
            // оптимальным вариантом является та, которая заканчивается раньше остальных доступных
            
            do
            {
                var nextActivities = GetNextActivities(DurableActivities, schedule.DurableActivities);
                
                nextBetterActivity = nextActivities.OrderBy(s=>s.EndTime).FirstOrDefault();

                if (nextBetterActivity == null) continue;
                
                schedule.AddActivity(nextBetterActivity);
                
            } while (nextBetterActivity != null);

            return schedule;
        }
        
        private List<Schedule> CalculateSchedules(Schedule schedule)
        {
            var nextActivities = GetNextActivities(_activities, schedule.Activities); 

            if (!nextActivities.Any())
                return new List<Schedule> {schedule};
            
            if (nextActivities.Count == 1) //чтобы не аллоцировать лишний Schedule
            {
                schedule.AddActivity(nextActivities.First());
                return CalculateSchedules(schedule);
            }

            var schedules = new List<Schedule>();
            foreach (var activity in nextActivities)
            {
                var newSchedule = schedule.Copy();
                newSchedule.AddActivity(activity);
                schedules.AddRange(CalculateSchedules(newSchedule));
            }

            return schedules;
        }

        private static List<T> GetNextActivities<T>(IEnumerable<T> allActivities, IEnumerable<T> currentActivities) where T : Activity
        {
            return allActivities
                .Where(s => s.IsDependingOn(currentActivities))
                .Except(currentActivities)
                .ToList();
        }
    }
}