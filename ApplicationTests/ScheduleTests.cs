using System;
using Application;
using NUnit.Framework;

namespace ApplicationTests
{
    [TestFixture]
    public class ScheduleTests
    {
        [Test]
        public void Schedule_WhenCalculateFinishTime_ShouldCalculateCorrectly()
        {
            //given
            var schedule = new Schedule(new[]
            {
                new DurableActivity(1, DateTimeHelper.SetTodayTime("12:00"), TimeSpan.FromMinutes(10)),
                new DurableActivity(2, DateTimeHelper.SetTodayTime("12:20"), TimeSpan.FromMinutes(15)),
                new DurableActivity(3, DateTimeHelper.SetTodayTime("12:10"), TimeSpan.FromMinutes(30))
            });

            var referenceEndTime = DateTimeHelper.SetTodayTime("13:05");
            
            //when
            var finishTime = schedule.GetFinishTime();
            
            //then
            Assert.AreEqual(referenceEndTime, finishTime);
        }

        [Test]
        public void Schedule_WhenCompareEqualsSchedules_ShouldBeEquals()
        {
            //given
            var referenceSchedule = new Schedule(new[]
            {
                new Activity(1),
                new Activity(2),
                new Activity(3)
            });
            
            var firstSchedule = new Schedule(new[]
            {
                new Activity(1),
                new Activity(2),
                new Activity(3)
            });
            
            var secondSchedule = new Schedule(new[]
            {
                new Activity(1),
                new Activity(3),
                new Activity(2)
            });

            //when
            var firstEquals = firstSchedule.IsEqualTo(referenceSchedule);
            var secondEquals = secondSchedule.IsEqualTo(referenceSchedule);

            //then
            Assert.True(firstEquals);
            Assert.False(secondEquals);
        }
    }
}