using System.Linq;

namespace BuildIndicator.Core
{
    public interface IBuildNotifier
    {
        void Notify(BuildNotification notification);
    }

    public static class NotifierConfigExtensions
    {
        public static IBuildNotifier IgnoreResting(this IBuildNotifier notifier)
        {
            return new NotificationTypeIgnorer(notifier, BuildStatus.Resting);
        }

        public static IBuildNotifier IgnoreBroken(this IBuildNotifier notifier)
        {
            return new NotificationTypeIgnorer(notifier, BuildStatus.Broken);
        }

        public static IBuildNotifier IgnoreBuilding(this IBuildNotifier notifier)
        {
            return new NotificationTypeIgnorer(notifier, BuildStatus.Building);
        }

        public static IBuildNotifier OnlyForBrokenBuilds(this IBuildNotifier notifier)
        {
            return new NotificationTypeIgnorer(notifier, BuildStatus.Resting, BuildStatus.Building);
        }

        public static IBuildNotifier OnlyWhenBuilding(this IBuildNotifier notifier)
        {
            return new NotificationTypeIgnorer(notifier, BuildStatus.Resting, BuildStatus.Broken);
        }

        public static IBuildNotifier OnlyWhenResting(this IBuildNotifier notifier)
        {
            return new NotificationTypeIgnorer(notifier, BuildStatus.Broken, BuildStatus.Building);
        }
    }

    public class NotificationTypeIgnorer : IBuildNotifier
    {
        private readonly BuildStatus[] IgnoreStatuses;
        private readonly IBuildNotifier inner;

        public NotificationTypeIgnorer(IBuildNotifier inner, params BuildStatus[] statiToIgnore)
        {
            this.IgnoreStatuses = statiToIgnore;
            this.inner = inner;
        }

        public void Notify(BuildNotification notification)
        {
            if (IgnoreStatuses.Contains(notification.Status))
                return;
            inner.Notify(notification);
        }
    }
}
