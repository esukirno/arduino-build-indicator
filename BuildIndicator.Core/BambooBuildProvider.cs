using System;

namespace BuildIndicator.Core
{
    public class NotificationProvider : INotificationProvider
    {
        private readonly string _planKey;
        private readonly Bamboo _bamboo;

        public NotificationProvider(string planKey, Bamboo client)
        {
            if (planKey == null) throw new ArgumentNullException("planKey");
            if (client == null) throw new ArgumentNullException("client");

            _planKey = planKey;
            _bamboo = client;
        }

        public bool TryGetNotificationSince(ResultCheckpoint lastCheckpoint, out BuildNotification notification)
        {
            notification = null;

            var plan = _bamboo.GetPlan(_planKey);
                
            if (plan.IsBuilding)
            {
                
                notification = new BuildNotification(
                    lastCheckpoint,
                    _planKey,
                    "Red 5 standing by",
                    BuildStatus.Building,triggeredBy: "Can't tell.");
                return true;
            }

            var result = _bamboo.GetLatestResultForPlan(_planKey);
            var currentCheckpoint = new ResultCheckpoint(result.Number);

            if (currentCheckpoint > lastCheckpoint)
            {
                if (!result.WasSuccessful())
                {
                    notification = new BuildNotification(
                        currentCheckpoint,
                        _planKey, "Broken!", BuildStatus.Broken, result.TriggeredBy);
                    return true;
                }

                notification = new BuildNotification(
                    currentCheckpoint,
                    _planKey, 
                    "Nothing to worry about. Now get back to work!",
                    BuildStatus.Resting, result.TriggeredBy);
                return true;
            }

            return false;
            
        }
    }

    public interface INotificationProvider
    {
        bool TryGetNotificationSince(ResultCheckpoint lastCheckpoint, out BuildNotification notification);
    }
}