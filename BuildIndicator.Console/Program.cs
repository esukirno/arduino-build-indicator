using System.Configuration;
using BuildIndicator.Core;
using BuildIndicator.Notifier.Arduino;
using BuildIndicator.Notifier.Growl;

namespace BuildIndicator.Console
{
    class Program
    {
        private static void Main(string[] args)
        {
            var planKey = ConfigurationManager.AppSettings["PlanKey"];
            var bambooUri = ConfigurationManager.AppSettings["BambooRestApiUri"];

            var notifications = new NotificationProvider(planKey, bambooUri);
            var notifier = new CompositeBuildNotifier(new GrowlBuildNotifier(),
                    new ArduinoBuildNotifier());
            var checkpointer = new ResultCheckpointer();

            var checkPoint = checkpointer.GetLast();

            BuildNotification notification = null;
            if (notifications.TryGetNotificationSince(checkPoint, out notification))
            {
                notifier.Notify(notification);
                checkpointer.Store(notification.Checkpoint);
            };
        }
    }
}
