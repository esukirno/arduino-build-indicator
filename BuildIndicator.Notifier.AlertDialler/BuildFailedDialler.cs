using System;
using System.Linq;
using System.Threading;
using BuildIndicator.AlertDialler.WebDialer;
using BuildIndicator.Core;

namespace BuildIndicator.AlertDialler
{
    public class BuildFailedDialler : IBuildNotifier 
    {
        private readonly Member[] members;

        private Timer _dialTimer;

        Credential cred = new Credential
        {
            password = "reception",
            userID = "152001"
        };

        UserProfile pro = new UserProfile
        {
            lineNumber = "152001",
            deviceName = "SEP00152B476BA2",
            user = "152001"
        };

        public BuildFailedDialler(params Member[] members)
        {
            if (members == null) throw new ArgumentNullException("members");
            this.members = members;
        }

        public void Notify(BuildNotification notification)
        {
            var triggeredBy = members.FirstOrDefault(m => m.UserName.Equals(notification.TriggeredBy));
            if (triggeredBy == null)
                return;
                
            CallMember(triggeredBy);
        }

        private void HangUp(object state)
        {
            using (var dialler = new WebdialerSoapServiceClient())
            {
                dialler.endCallSoap(cred, pro);
                dialler.Close();
            }
        }

        private void CallMember(Member triggeredBy)
        {
            using (var dialler = new WebdialerSoapServiceClient())
            {
                dialler.makeCallSoap(cred, triggeredBy.PhoneNumber, pro);
                dialler.Close();
            }

            _dialTimer = new Timer(HangUp);
            _dialTimer.Change(TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(-1));
        }
    }
}
