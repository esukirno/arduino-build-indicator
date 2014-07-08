using Growl.Connector;

namespace IsBambooBuildBrokenReader
{
    public class GrowlBuildNotifier : IBuildNotifier
    {
        private GrowlConnector _growl;
        private NotificationType _notificationType;
        private Application _application;

        public GrowlBuildNotifier()
        {
            _growl = new GrowlConnector();

            _notificationType = new NotificationType("Bamboo Notification");
            _application = new Application(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

            _growl.Register(_application, new [] { _notificationType });           
        }

        public void Notify(BuildNotification notification)
        {
            var growlNotification = new Notification(_application.Name, _notificationType.Name, notification.Name,
                notification.Name + " is " + notification.Status, notification.Description);
            _growl.Notify(growlNotification);
        }
    }
}