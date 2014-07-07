using System;
using System.Configuration;
using System.IO.Ports;
using Newtonsoft.Json;

namespace IsBambooBuildBrokenReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var planKey = ConfigurationManager.AppSettings["PlanKey"];

            var bamboo = new Bamboo("http://tools-bamboo:8085/rest/api/latest/");
            
            var plan = bamboo.GetPlan(planKey);

            if (plan.IsBuilding)
            {
                AlertBuildBuilding();
            }

            var result = bamboo.GetLatestResultForPlan(planKey);

            if (!result.WasSuccessful())
            {
                AlertBuildBroken();
            }
            else
            {
                AlertBuildResting();
            }
        }

        static void AlertBuildResting()
        {
            SendMessage("0");
        }

        static void AlertBuildBroken()
        {
            SendMessage("1");
        }

        static void AlertBuildBuilding()
        {
            SendMessage("2");
        }

        static void SendMessage(string message)
        {
            Console.WriteLine("Sending Message: {0}", message);
            try
            {
                using (var outputPort = new SerialPort("COM4", 9600))
                {
                    outputPort.Open();
                    outputPort.Write(message);
                    outputPort.Close();
                }
            }
            catch
            {
            }
        }
    }
}
