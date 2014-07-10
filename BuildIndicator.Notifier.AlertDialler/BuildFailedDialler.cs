using System;
using System.Linq;
using BuildIndicator.AlertDialler.WebDialer;
using BuildIndicator.Core;

namespace BuildIndicator.AlertDialler
{
    public class BuildFailedDialler : IBuildNotifier 
    {
        private readonly Member[] members;

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

            using (var dialler = new WebDialer.WebdialerSoapServiceClient())
            {
                var cred = new Credential
                {
                    password = "reception",
                    userID = "152001"
                };
                var pro = new UserProfile
                {
                    lineNumber = "152001",
                    deviceName = "SEP00152B476BA2",
                    user = "152001"
                };
                dialler.makeCallSoap(cred, triggeredBy.PhoneNumber, pro);
                dialler.Close();
            }
        }
    }

    public class Member
    {
        public Member(string userName, string phoneNumber)
        {
            if (string.IsNullOrEmpty( userName)) throw new ArgumentNullException("userName");
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("phoneNumber");

            UserName = userName;
            PhoneNumber = phoneNumber;
        }

        public string UserName { get; private set; }
        public string PhoneNumber { get; private set; }
    }
}
