namespace BuildIndicator.Core
{
    public interface IBuildNotifier
    {
        void Notify(BuildNotification notification);
    }

    public class ChromeBuildNotifier : IBuildNotifier
    {
        public void Notify(BuildNotification notification)
        {
            
        }
    }

}
