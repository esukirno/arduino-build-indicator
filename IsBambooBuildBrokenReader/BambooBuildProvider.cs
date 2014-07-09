using System;

namespace BuildIndicator.Core
{
    public class NotificationProvider
    {
        private readonly string _planKey;
        private readonly string _bambooLatestRestApiUri;

        public NotificationProvider(string planKey, string bambooLatestRestApiUri)
        {
            if (planKey == null) throw new ArgumentNullException("planKey");
            if (bambooLatestRestApiUri == null) throw new ArgumentNullException("bambooLatestRestApiUri");

            _planKey = planKey;
            _bambooLatestRestApiUri = bambooLatestRestApiUri;
        }

        public bool TryGetNotificationSince(ResultCheckpoint lastCheckpoint, out BuildNotification notification)
        {
            notification = null;

            using (var bamboo = new Bamboo(_bambooLatestRestApiUri))
            {
                var plan = bamboo.GetPlan(_planKey);
                
                if (plan.IsBuilding)
                {
                    notification = new BuildNotification(
                        lastCheckpoint,
                        _planKey,
                        "Red 5 standing by",
                        BuildStatus.Building);
                    return true;
                }

                var result = bamboo.GetLatestResultForPlan(_planKey);
                var currentCheckpoint = new ResultCheckpoint(result.Number);

                if (currentCheckpoint > lastCheckpoint)
                {
                    if (!result.WasSuccessful())
                    {
                        notification = new BuildNotification(
                            currentCheckpoint,
                            _planKey, "Broken!", BuildStatus.Broken);
                        return true;
                    }

                    notification = new BuildNotification(
                        currentCheckpoint,
                        _planKey, "Nothing to worry about. Now get back to work!", BuildStatus.Resting);
                    return true;
                }

                return false;
            }
        }
    }
}