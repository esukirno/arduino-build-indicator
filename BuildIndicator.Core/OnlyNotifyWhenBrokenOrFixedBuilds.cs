using System;
using System.Collections.Generic;

namespace BuildIndicator.Core
{
    public class OnlyNotifyWhenBrokenOrFixedBuilds : IBuildNotifier
    {
        private enum State
        {
            NormalOperation,
            Broken,
            Fixing,
            Fixed,
        }

        private State _state;

        private readonly Dictionary<State, Action<BuildNotification>> handlers = new Dictionary<State, Action<BuildNotification>>();

        private readonly IBuildNotifier notifier;

        public OnlyNotifyWhenBrokenOrFixedBuilds(IBuildNotifier notifier)
        {
            this.notifier = notifier;
            this.handlers.Add(State.Broken, AlertBroken);
            this.handlers.Add(State.Fixing, AlertFixing);
            this.handlers.Add(State.Fixed, AlertFixed);
            this.handlers.Add(State.NormalOperation, IgnoreNormalOperations);
        }

        public void Notify(BuildNotification notification)
        {
            MoveState(notification.Status);
            this.handlers[_state](notification);
        }

        private void MoveState(BuildStatus newStatus)
        {
            if (newStatus == BuildStatus.Building || newStatus == BuildStatus.Resting &&
                (_state == State.Fixed || _state == State.NormalOperation))
            {
                _state = State.NormalOperation;
                return;
            }

            if (newStatus == BuildStatus.Building &&
                _state == State.Broken)
            {
                _state = State.Fixing;
                return;
            }

            _state = newStatus == BuildStatus.Broken ? State.Broken : State.Fixed;
        }

        private void IgnoreNormalOperations(BuildNotification notification)
        {
            
        }

        private void AlertBroken(BuildNotification notification)
        {
            notifier.Notify(notification);
        }

        private void AlertFixing(BuildNotification notification)
        {
            notifier.Notify(notification);
        }

        private void AlertFixed(BuildNotification notification)
        {
            notifier.Notify(notification);
        }
    }
}