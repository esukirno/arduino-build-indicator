using System;
using System.Collections.Generic;
using BuildIndicator.AlertDialler;
using BuildIndicator.Core;
using BuildIndicator.Notifier.Arduino;
using BuildIndicator.Notifier.Growl;

namespace BuildIndicator.TopShelf
{
    public class Configure
    {
        private readonly Configuration config;

        private List<IBuildNotifier> notifiers = new List<IBuildNotifier>();

        public class Configuration
        {
            public string BambooUri { get; set; }
            public string PlanKey { get; set; }
        }

        private Configure(Configuration config)
        {
            this.config = config;
        }

        public static Configure Dispatcher(Action<Configuration> configure)
        {
            var config = new Configuration();
            configure(config);
            return new Configure(config);
        }

        public Configure NotifyMembers(params Member[] members)
        {
            notifiers.Add(new BuildFailedDialler(members).OnlyForBrokenBuilds());
            return this;
        }

        public Configure WithGrowl()
        {
            notifiers.Add(new GrowlBuildNotifier());
            return this;
        }

        public Configure WithBuildLights()
        {
            notifiers.Add(new ArduinoBuildNotifier());
            return this;
        }

        public BuildNotificationDispatcher Start()
        {
            var bamboo = new Bamboo(config.BambooUri);
            var notifications = new NotificationProvider(config.PlanKey, bamboo);
            var checkpointer = new ResultCheckpointer();
            var dispatcher = new BuildNotificationDispatcher(notifications, checkpointer, new OnlyNotifyWhenStateHasChanged(new OnlyNotifyWhenBrokenOrFixedBuilds(new CompositeBuildNotifier(notifiers.ToArray()))));

            dispatcher.Start();

            return dispatcher;
        }
    }
}