using System;
using System.Collections.Generic;

namespace BuildIndicator.Core
{
    public class OnlyNotifyWhenBrokenOrFixedBuilds : IBuildNotifier
    {
        private BuildState _state;

        public OnlyNotifyWhenBrokenOrFixedBuilds(IBuildNotifier notifier)
        {
            _state = new NormalOperations(notifier);
        }

        public void Notify(BuildNotification notification)
        {
            if (notification.Status == BuildStatus.Broken)
                _state = _state.HandleBroken(notification);

            if (notification.Status == BuildStatus.Resting)
                _state = _state.HandleResting(notification);

            if (notification.Status == BuildStatus.Building)
                _state = _state.HandleBuilding(notification);
        }
    }

    public abstract class BuildState
    {
        public abstract BuildState HandleBuilding(BuildNotification notification);
        public abstract BuildState HandleResting(BuildNotification notification);
        public abstract BuildState HandleBroken(BuildNotification notification);
    }

    public class NormalOperations : BuildState
    {
        private readonly IBuildNotifier notifier;

        public NormalOperations(IBuildNotifier notifier)
        {
            this.notifier = notifier;
        }

        public override BuildState HandleBuilding(BuildNotification notification)
        {
            return this;
        }

        public override BuildState HandleResting(BuildNotification notification)
        {
            return this;
        }

        public override BuildState HandleBroken(BuildNotification notification)
        {
            notifier.Notify(notification);
            return new BrokenBuild(notifier);
        }
    }

    public class BrokenBuild : BuildState
    {
        private readonly IBuildNotifier notifier;

        public BrokenBuild(IBuildNotifier notifier)
        {
            this.notifier = notifier;
        }

        public override BuildState HandleBuilding(BuildNotification notification)
        {
            notifier.Notify(notification);
            return new FixingBuild(notifier);
        }

        public override BuildState HandleResting(BuildNotification notification)
        {
            //logically we shuold hit building first and move to fixing - but just incase, lets notify and return to normal.
            notifier.Notify(notification);
            return new NormalOperations(notifier);
        }

        public override BuildState HandleBroken(BuildNotification notification)
        {
            return this;
        }
    }

    public class FixingBuild : BuildState
    {
        private readonly IBuildNotifier notifier;

        public FixingBuild(IBuildNotifier notifier)
        {
            this.notifier = notifier;
        }

        public override BuildState HandleBuilding(BuildNotification notification)
        {
            //still fixing;
            return this;
        }

        public override BuildState HandleResting(BuildNotification notification)
        {
            //fixed!
            notifier.Notify(notification);
            return new NormalOperations(notifier);
        }

        public override BuildState HandleBroken(BuildNotification notification)
        {
            notifier.Notify(notification);
            return new BrokenBuild(notifier);
        }
    }
}