using System;
using System.Threading;
using BuildIndicator.AlertDialler.WebDialer;
using BuildIndicator.Core;

namespace BuildIndicator.AlertDialler
{
    public class BuildFailedDialler : IBuildNotifier 
    {
        public void Notify(BuildNotification notification)
        {
            var dialler = new WebDialer.WebdialerSoapServiceClient();
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
            var response = dialler.makeCallSoap(cred,"092317406", pro);
            Thread.Sleep(TimeSpan.FromSeconds(10));
            dialler.endCallSoap(cred, pro);
            dialler.Close();
        }
    }
}
