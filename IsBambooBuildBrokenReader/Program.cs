using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Configuration;

namespace IsBambooBuildBrokenReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = ConfigurationManager.AppSettings["BambooBuildPlanRssFeedUrl"];
            var username = ConfigurationManager.AppSettings["BambooUsername"];
            var password = ConfigurationManager.AppSettings["BambooPassword"];

            using (var webClient = new WebClient())
            {
                webClient.Credentials = new NetworkCredential(username, password);
                var data = webClient.DownloadString(url);

                using (var reader = XmlReader.Create(new StringReader(data)))
                {
                    var feed = SyndicationFeed.Load(reader);

                    if (feed.Items.Any())
                    {
                        dynamic buildStatus = feed.Items.FirstOrDefault();
                        if (buildStatus.Title.Text.Contains("FAILED"))
                        {
                            TurnOnSiren();
                        }
                        else
                        {
                            TurnOffSiren();
                        }
                    }
                }
            }            
        }

        static void TurnOnSiren()
        {
            using (var outputPort = new SerialPort("COM4", 9600))
            {
                outputPort.Open();
                outputPort.Write("1");
                outputPort.Close();
            }
        }

        static void TurnOffSiren()
        {
            using (var outputPort = new SerialPort("COM4", 9600))
            {
                outputPort.Open();
                outputPort.Write("0");
                outputPort.Close();
            }
        }
    }
}
