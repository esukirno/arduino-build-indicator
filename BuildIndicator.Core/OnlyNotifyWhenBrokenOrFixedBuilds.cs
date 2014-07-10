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

        private readonly Dictionary<State, Action<BuildNotification>> handlers =
            new Dictionary<State, Action<BuildNotification>>();

        private readonly IBuildNotifier notifier;

        public OnlyNotifyWhenBrokenOrFixedBuilds(IBuildNotifier notifier)
        {
            this.notifier = notifier;
            this.handlers.Add(State.Broken, Alert);
            this.handlers.Add(State.Fixing, Alert);
            this.handlers.Add(State.Fixed, Alert);
            this.handlers.Add(State.NormalOperation, Ignore);
        }

        public void Notify(BuildNotification notification)
        {
            MoveState(notification.Status);
            this.handlers[_state](notification);
        }

        private void MoveState(BuildStatus newStatus)
        {
            if (newStatus == BuildStatus.Building &&
                (_state == State.Fixed || _state == State.NormalOperation))
                _state = State.NormalOperation;

            if (newStatus == BuildStatus.Building &&
                _state == State.Broken)
                _state = State.Fixing;

            _state = newStatus == BuildStatus.Broken ? State.Broken : State.Fixed;
        }

        private void Ignore(BuildNotification notification)
        {

        }

        private void Alert(BuildNotification notification)
        {
            notifier.Notify(notification);
        }
    }
}