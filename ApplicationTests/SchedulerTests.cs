using System;
using System.Collections.Generic;
using System.Linq;
using Application;
using NUnit.Framework;

namespace ApplicationTests
{
    [TestFixture]
    public class SchedulerTests
    {
        private Schedule _schedule;
        private Schedule[] PossibleSchedules;
        
        [SetUp]
        public void SetUp()
        {
            var activities = new Dictionary<long, Activity>
            {
                {1, new DurableActivity(1, DateTimeHelper.SetTodayTime("12:00"), TimeSpan.FromMinutes(10))}
            };
            
            activities.Add(2,
                new DurableActivity(2, new[]
                    {
                        activities[1]
                    },
                    DateTimeHelper.SetTodayTime("12:10"),
                    TimeSpan.FromMinutes(15)
                )
            );

            activities.Add(3,
                new DurableActivity(3, new[]
                    {
                        activities[1]
                    },
                    DateTimeHelper.SetTodayTime("12:10"),
                    TimeSpan.FromMinutes(30)
                )
            );
            
            activities.Add(4, new DurableActivity(4, new[]
                    {
                        activities[2]
                    },
                    DateTimeHelper.SetTodayTime("12:35"),
                    TimeSpan.FromMinutes(10)
                )
            );

            activities.Add(5, new DurableActivity(5, new[]
                    {
                        activities[2],
                        activities[3]
                    },
                    DateTimeHelper.SetTodayTime("12:00"),
                    TimeSpan.FromMinutes(5)
                )
            );

            _schedule = new Schedule(activities.Select(s => s.Value));

            PossibleSchedules = new []
            {
                new Schedule(new List<Activity>
                {
                    activities[1],
                    activities[2],
                    activities[3],
                    activities[4],
                    activities[5]
                }),
                new Schedule(new List<Activity>
                {
                    activities[1],
                    activities[2],
                    activities[3],
                    activities[5],
                    activities[4]
                }),
                new Schedule(new List<Activity>
                {
                    activities[1],
                    activities[3],
                    activities[2],
                    activities[4],
                    activities[5]
                }),
                new Schedule(new List<Activity>
                {
                    activities[1],
                    activities[3],
                    activities[2],
                    activities[5],
                    activities[4]
                }),
                new Schedule(new List<Activity>
                {
                    activities[1],
                    activities[2],
                    activities[4],
                    activities[3],
                    activities[5]
                }),
            };
        }

        [Test]
        public void Scheduler_WhenCalculatingAllSchedules_ShouldCalculateAllPossibleSchedules()
        {
            //SUT
            var scheduler = new Scheduler(_schedule.Activities.ToArray());
            
            //when
            var schedules = scheduler.GetSchedules();

            var equalsAllPossibleSchedules = schedules.All(
                schedule => PossibleSchedules.Any(s => s.IsEqualTo(schedule))
            );

            //then
            Assert.AreEqual(PossibleSchedules.Length, schedules.Count);
            Assert.True(equalsAllPossibleSchedules);
        }

        [Test]
        public void Scheduler_WhenFindingShortestSchedule_ShouldFindShortestFromAllPossibleSchedules()
        {
            //SUT
            var scheduler = new Scheduler(_schedule.Activities.ToArray());
            
            //when
            var schedules = scheduler.GetSchedules();
            var shortestSchedule = scheduler.FindShortestSchedule();

            schedules.Sort(new ScheduleComparer());
            var referenceShortestSchedule = schedules.First();

            var shortestFinishTime = shortestSchedule.GetFinishTime();
            var referenceFinishTime = referenceShortestSchedule.GetFinishTime();
            var equalsReference = shortestSchedule.IsEqualTo(referenceShortestSchedule);
            
            //then
            Assert.AreEqual(referenceFinishTime, shortestFinishTime);
            Assert.True(equalsReference);
        }
    }
}