using System;

namespace IsBambooBuildBrokenReader
{
    public class StateAwareNotifier : IBuildNotifier
    {
        private BuildStatus? status;

        private readonly IBuildNotifier notifier;

        public StateAwareNotifier(IBuildNotifier notifier)
        {
            this.notifier = notifier;
        }

        public void Notify(BuildNotification notification)
        {
            if (!status.HasValue || notification.Status != status.Value)
            {
                notifier.Notify(notification);
            }

            status = notification.Status;
        }
    }

    public class CompositeBuildNotifier : IBuildNotifier
    {
        private readonly IBuildNotifier[] _notifiers;

        public CompositeBuildNotifier(params IBuildNotifier[] notifiers)
        {
            _notifiers = notifiers;
        }

        public void Notify(BuildNotification notification)
        {
            Array.ForEach(_notifiers, notifier => notifier.Notify(notification));
        }
    }
}