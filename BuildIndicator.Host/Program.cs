using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using BuildIndicator.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace BuildIndicator.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                string line;
                do
                {
                    line = Console.ReadLine();
                    Console.WriteLine("Publish dummy notification");
                    var notification = new BuildNotification(new ResultCheckpoint(1), "Build 1", "No worries be happy",
                        BuildStatus.Broken);
                    var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    context.Clients.All.BuildNotificationPublished(notification);

                } while (line != null);
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
    public class MyHub : Hub
    {
        public void BuildNotificationPublished(BuildNotification notification)
        {
            Clients.All.BuildNotificationPublished(notification);
        }
    }
}
