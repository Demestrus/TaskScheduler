using System;
using System.Collections.Generic;
using System.Linq;
using Application;

namespace ConsoleSecondTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //given
            var activities = new Dictionary<long, Activity>
            {
                {1, new DurableActivity(1, SetTodayDateTime("12:00"), TimeSpan.FromMinutes(15))},
                {2, new DurableActivity(2, SetTodayDateTime("12:00"), TimeSpan.FromMinutes(5))}
            };


            activities.Add(3,
                new DurableActivity(3, new[]
                    {
                        activities[1]
                    },
                    SetTodayDateTime("12:10"),
                    TimeSpan.FromMinutes(10)
                )
            );

            activities.Add(4,
                new DurableActivity(4, new[]
                    {
                        activities[2],
                        activities[3]
                    },
                    SetTodayDateTime("12:20"),
                    TimeSpan.FromMinutes(10)
                )
            );
            
            activities.Add(5, new DurableActivity(5, new[]
                    {
                        activities[4]
                    },
                    SetTodayDateTime("12:50"),
                    TimeSpan.FromMinutes(5)
                )
            );

            activities.Add(6, new DurableActivity(6, new[]
                    {
                        activities[4]
                    },
                    SetTodayDateTime("13:00"),
                    TimeSpan.FromMinutes(5)
                )
            );

            activities.Add(7, new DurableActivity(7, new[]
                    {
                        activities[5]
                    },
                    SetTodayDateTime("13:45"),
                    TimeSpan.FromMinutes(5)
                )
            );

            activities.Add(8, new DurableActivity(8, new[]
                    {
                        activities[5]
                    },
                    SetTodayDateTime("13:20"),
                    TimeSpan.FromMinutes(15)
                )
            );

            activities.Add(9, new DurableActivity(9, new[]
                    {
                        activities[6]
                    },
                    SetTodayDateTime("13:30"),
                    TimeSpan.FromMinutes(15)
                )
            );

            try
            {
                //SUT
                var scheduler = new Scheduler(activities.Select(s => s.Value).ToArray());

                //Result
                var schedule = scheduler.FindShortestSchedule();

                foreach (var activity in schedule.Activities)
                {
                    Console.Write($"{activity.Id} ");
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static DateTime SetTodayDateTime(string timeSpan) => DateTime.UtcNow.Date.Add(TimeSpan.Parse(timeSpan));
    }
}