using System;

namespace IsBambooBuildBrokenReader
{
    public class NotificationProvider : IDisposable
    {
        private readonly string _planKey;
        private readonly string _bambooLatestRestApiUri;
        private readonly Bamboo _bamboo;

        public NotificationProvider(string planKey, string bambooLatestRestApiUri)
        {
            if (planKey == null) throw new ArgumentNullException("planKey");
            if (bambooLatestRestApiUri == null) throw new ArgumentNullException("bambooLatestRestApiUri");

            _planKey = planKey;
            _bambooLatestRestApiUri = bambooLatestRestApiUri;
            _bamboo = new Bamboo(_bambooLatestRestApiUri);
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
                    BuildStatus.Building);
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

        public void Dispose()
        {
            _bamboo.Dispose();
        }
    }
}