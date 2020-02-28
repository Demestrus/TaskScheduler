using System;
using System.Collections.Generic;
using System.Linq;
using Application;

namespace ConsoleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var activities = new Dictionary<long, Activity>
            {
                {1, new Activity(1)}, 
                {2, new Activity(2)}
            };

            activities.Add(3,
                new Activity(3, new[]
                {
                    activities[1]
                })
            );
            
            activities.Add(4, 
                new Activity(4, new[]
                {
                    activities[2],
                    activities[3]
                })
            );
            
            activities.Add(5, new Activity(5, new[]
                {
                    activities[4]
                })
            );

            activities.Add(6, new Activity(6, new[]
                {
                    activities[4]
                })
            );

            activities.Add(7, new Activity(7, new[]
                {
                    activities[5]
                })
            );

            activities.Add(8, new Activity(8, new[]
                {
                    activities[5]
                })
            );

            activities.Add(9, new Activity(9, new[]
                {
                    activities[6]
                })
            );

            try
            {
                var scheduler = new Scheduler(activities.Select(s => s.Value).ToArray());
                
                var result = scheduler.GetSchedules();

                foreach (var schedule in result)
                {
                    foreach (var activity in schedule.Activities)
                    {
                        Console.Write($"{activity.Id} ");
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}