using System.Runtime.InteropServices;

namespace BuildIndicator.Core
{
    public class OnlyNotifyWhenStateHasChanged : IBuildNotifier
    {
        private BuildStatus? status;

        private readonly IBuildNotifier notifier;

        public OnlyNotifyWhenStateHasChanged(IBuildNotifier notifier)
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
}