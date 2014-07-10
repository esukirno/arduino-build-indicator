using BuildIndicator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildIndicator.Core;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;
using Xunit.Extensions;

namespace Specifications
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
            
        }
    }

    public class CompositeBuildNotifierSpecification
    {
        [Theory, AutoMoqData]
        public void ShoudNotifyAllNotifiers(BuildNotification notification, 
            Mock<IBuildNotifier> notifier1, Mock<IBuildNotifier> notifier2, Mock<IBuildNotifier> notifier3)
        {

            var sut = new CompositeBuildNotifier(notifier1.Object, notifier2.Object, notifier3.Object);

            sut.Notify(notification);

            notifier1.Verify(x => x.Notify(notification), Times.Once());
            notifier2.Verify(x => x.Notify(notification), Times.Once());
            notifier3.Verify(x => x.Notify(notification), Times.Once());
        }
    }
}
