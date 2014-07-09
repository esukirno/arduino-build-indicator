using System;
using System.Configuration;
using System.Threading;
using IsBambooBuildBrokenReader;

namespace BuildIndicator.Console
{
    class Program
    {
        private static void Main(string[] args)
        {
            new Program().Run();
        }

        public void Run()
        {
            System.Console.WriteLine("Press any key to stop the service.");
            var planKey = ConfigurationManager.AppSettings["PlanKey"];
            var bambooUri = ConfigurationManager.AppSettings["BambooRestApiUri"];

            var notifications = new NotificationProvider(planKey, bambooUri);
            var notifier = new StateAwareNotifier(new CompositeBuildNotifier(new GrowlBuildNotifier(),
                new ArduinoBuildNotifier()));
            var checkpointer = new ResultCheckpointer();

            try
            {
                while (!System.Console.KeyAvailable)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    var checkPoint = checkpointer.GetLast();

                    BuildNotification notification = null;
                    if (notifications.TryGetNotificationSince(checkPoint, out notification))
                    {
                        notifier.Notify(notification);
                        checkpointer.Store(notification.Checkpoint);
                    }
                }
            }
            finally
            {
                notifications.Dispose();
            }
        }
    }
}
