using Application;
using NUnit.Framework;

namespace ApplicationTests
{
    [TestFixture]
    public class ActivityTests
    {
        [Test]
        public void Activity_WhenHasDepending_ShouldDependsOn()
        {
            //given
            var dependActivity = new Activity(1);
            var notDependActivity = new Activity(2);
            var activity = new Activity(3, new[] {dependActivity});
            
            //when
            var depends = activity.IsDependingOn(new[] {dependActivity});
            var notDepends = activity.IsDependingOn(new[] {notDependActivity});
            
            //then
            Assert.IsTrue(depends);
            Assert.IsFalse(notDepends);
        }

        [Test]
        public void Activity_WhenHasNoDependences_ShouldBeIndependent()
        {
            //given
            var activity = new Activity(1);
            
            //when
            var depends = activity.IsDependingOn(new Activity[]{});
            var isIndependent = activity.IsIndependent;
            
            //then
            Assert.IsTrue(depends);
            Assert.IsTrue(isIndependent);
        }
    }
}