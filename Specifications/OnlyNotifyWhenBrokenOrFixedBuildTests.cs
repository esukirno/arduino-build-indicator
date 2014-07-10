using BuildIndicator.Core;
using Xunit;

namespace Specifications
{
    public class OnlyNotifyWhenBrokenOrFixedBuildTests
    {
        private SpyNotifier spy;
        private OnlyNotifyWhenBrokenOrFixedBuilds sut;

        public OnlyNotifyWhenBrokenOrFixedBuildTests()
        {
            spy = new SpyNotifier();
            sut = new OnlyNotifyWhenBrokenOrFixedBuilds(spy);
        }

        [Fact]
        public void When_resting_and_building_notification_is_received_should_not_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Building));
            Assert.False(spy.WasNotified);
        }

        [Fact]
        public void When_resting_and_resting_notification_is_received_then_should_not_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Resting));
            Assert.False(spy.WasNotified);
        }

        [Fact]
        public void When_resting_and_build_broken_notification_is_received_then_should_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Broken));
            Assert.True(spy.WasNotified);
        }

        [Fact]
        public void When_building_and_resting_notification_is_received_then_should_not_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Building));
            sut.Notify(MakeNotification(BuildStatus.Resting));

            Assert.False(spy.WasNotified);
        }

        [Fact]
        public void When_building_and_broken_notification_is_received_then_should_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Building));
            sut.Notify(MakeNotification(BuildStatus.Broken));

            Assert.True(spy.WasNotified);
        }

        [Fact]
        public void When_building_and_building_notification_is_received_then_should_not_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Building));
            sut.Notify(MakeNotification(BuildStatus.Building));

            Assert.False(spy.WasNotified);
        }

        [Fact]
        public void When_fixing_and_resting_notification_is_received_then_should_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Building));
            sut.Notify(MakeNotification(BuildStatus.Broken));
            sut.Notify(MakeNotification(BuildStatus.Building));
            spy.Forget();
            
            sut.Notify(MakeNotification(BuildStatus.Resting));

            Assert.True(spy.WasNotified);
        }

        [Fact]
        public void When_fixing_and_broken_notification_is_received_then_should_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Building));
            sut.Notify(MakeNotification(BuildStatus.Broken));
            sut.Notify(MakeNotification(BuildStatus.Building));
            spy.Forget();

            sut.Notify(MakeNotification(BuildStatus.Broken));

            Assert.True(spy.WasNotified);
        }

        [Fact]
        public void When_fixing_and_building_notification_is_received_then_should_not_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Building));
            sut.Notify(MakeNotification(BuildStatus.Broken));
            sut.Notify(MakeNotification(BuildStatus.Building));
            spy.Forget();

            sut.Notify(MakeNotification(BuildStatus.Building));

            Assert.False(spy.WasNotified);
        }

        [Fact]
        public void When_broken_and_building_notification_is_received_then_should_Notify()
        {
            sut.Notify(MakeNotification(BuildStatus.Building));
            sut.Notify(MakeNotification(BuildStatus.Broken));
            
            spy.Forget();

            sut.Notify(MakeNotification(BuildStatus.Building));

            Assert.True(spy.WasNotified);
        }

        [Fact]
        public void When_broken_and_resting_notification_is_received_then_should_Notify()
        {
            //this logically shouldnt occur but if it does - probably should alert to make sure the lights turn off.
            sut.Notify(MakeNotification(BuildStatus.Building));
            sut.Notify(MakeNotification(BuildStatus.Broken));

            spy.Forget();

            sut.Notify(MakeNotification(BuildStatus.Resting));

            Assert.True(spy.WasNotified);
        }



        private BuildNotification MakeNotification(BuildStatus status)
        {
            return new BuildNotification(null, "test", "test", status, "the tester");
        }
        
    }

    public class SpyNotifier : IBuildNotifier
    {
        public bool WasNotified;
        public void Notify(BuildNotification notification)
        {
            WasNotified = true;
        }

        public void Forget()
        {
            WasNotified = false;
        }
    }
}