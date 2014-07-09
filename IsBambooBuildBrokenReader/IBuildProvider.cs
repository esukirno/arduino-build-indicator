namespace IsBambooBuildBrokenReader
{
    public interface IBuildProvider
    {
        BuildNotification GetLatestBuildNotification();
    }
}