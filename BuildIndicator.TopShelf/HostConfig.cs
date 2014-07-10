using System;
using Idecom.Host.Interfaces;
using Topshelf;
using Topshelf.HostConfigurators;


namespace BuildIndicator.TopShelf
{
    public class IdecomHost : HostedService
    {
        public override bool Start(HostControl hostControl)
        {
            hostControl.RequestAdditionalTime(TimeSpan.FromSeconds(10));
            return true;
        }

        public override bool Stop(HostControl hostControl)
        {
            return true;
        }

        public override void OverrideDefaultConfiguration(HostConfigurator configurator)
        {
        }

        public override bool Pause(HostControl hostControl)
        {
            return true;
        }

        public override bool Continue(HostControl hostControl)
        {
            return true;
        }
    }
}
