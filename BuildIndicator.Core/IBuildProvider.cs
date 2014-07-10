namespace BuildIndicator.Core
{
    public interface IBuildProvider
    {
        BuildNotification GetNotificationsSince(ResultCheckpoint checkpoint);
    }
}