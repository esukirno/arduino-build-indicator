using System;
using System.Threading;

namespace BuildIndicator.Core
{
    public class BuildNotificationDispatcher
    {
        private readonly INotificationProvider notifications;
        private readonly ICheckPointer checkPointer;
        private readonly IBuildNotifier notifier;

        private Thread _thread;
        private ManualResetEventSlim _stopped = new ManualResetEventSlim();
        private volatile bool _stop;

        public BuildNotificationDispatcher(
            INotificationProvider notifications,
            ICheckPointer checkPointer,
            IBuildNotifier notifier)
        {
            this.notifications = notifications;
            this.checkPointer = checkPointer;
            this.notifier = notifier;
        }

        public void Start()
        {
            if (_thread != null)
                throw new InvalidOperationException("Already a thread running.");
            _stopped.Reset();

            _thread = new Thread(Dispatch) { IsBackground = true };
            _thread.Start();
        }

        private void Dispatch()
        {
            while (!_stop)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));

                var checkPoint = checkPointer.GetLast();

                BuildNotification notification = null;
                if (notifications.TryGetNotificationSince(checkPoint, out notification))
                {
                    notifier.Notify(notification);
                    checkPointer.Store(notification.Checkpoint);
                }
            }

            _stopped.Set();
        }

        public void Stop()
        {
            _stop = true;
            if (!_stopped.Wait(TimeSpan.FromSeconds(15)))
                throw new TimeoutException(string.Format("Unable to stop dispatcher"));
        }
    }
}