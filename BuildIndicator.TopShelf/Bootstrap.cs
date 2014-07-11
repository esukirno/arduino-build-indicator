using System.Configuration;
using System.Net;
using BuildIndicator.AlertDialler;
using BuildIndicator.Core;
using BuildIndicator.Notifier.Arduino;
using BuildIndicator.Notifier.Growl;
using Idecom.Host.Interfaces;

namespace BuildIndicator.TopShelf
{
    public class Bootstrap : IWantToStartAfterServiceStarts
    {
        private BuildNotificationDispatcher dispatcher;
        public void AfterStart()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            var planKey = ConfigurationManager.AppSettings["PlanKey"];
            var bambooUri = ConfigurationManager.AppSettings["BambooRestApiUri"];

            var bamboo = new Bamboo(bambooUri);
            var notifications = new NotificationProvider(planKey, bamboo);
            var notifier =
                new OnlyNotifyWhenStateHasChanged(
                new OnlyNotifyWhenBrokenOrFixedBuilds(
                new CompositeBuildNotifier(
                    new GrowlBuildNotifier(),
                    new ArduinoBuildNotifier(),
                    new BuildFailedDialler(
                        new Member("joseph_flood", "092317406"), 
                        new Member("evgeny.komarevtsev", "152002"),
                        new Member("jradwan", "0292317641"),
                        new Member("muditha.dissanayake", "092317644"),
                        new Member( "esukirno","092317498"),
                        new Member("saurabh.sharma", "0923177682"),
                        new Member("tim_servcorp", "092317671")
                        ).OnlyForBrokenBuilds()
                )));
            var checkpointer = new ResultCheckpointer();
            dispatcher = new BuildNotificationDispatcher(notifications, checkpointer, notifier);

            dispatcher.Start();
        }

        public void BeforeStop()
        {
            dispatcher.Stop();
        }
    }
}
