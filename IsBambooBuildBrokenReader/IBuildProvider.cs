namespace IsBambooBuildBrokenReader
{
    public interface IBuildProvider
    {
        BuildNotification GetNotificationsSince(ResultCheckpoint checkpoint);
    }
}