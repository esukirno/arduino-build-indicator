using System;

namespace BuildIndicator.Core
{
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