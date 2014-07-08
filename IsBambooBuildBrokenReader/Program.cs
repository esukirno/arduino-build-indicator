using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace IsBambooBuildBrokenReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var planKey = ConfigurationManager.AppSettings["PlanKey"];            

            BuildNotification notification;
            using (var bamboo = new Bamboo("http://tools-bamboo:8085/rest/api/latest/"))
            {
                var plan = bamboo.GetPlan(planKey);

                if (plan.IsBuilding)
                {
                    notification = new BuildNotification(planKey, BuildStatus.Building, "Red 5 standing by");
                }

                var result = bamboo.GetLatestResultForPlan(planKey);

                if (!result.WasSuccessful())
                {
                    notification = new BuildNotification(planKey, BuildStatus.Broken, "Success! Row inserted");
                }
                else
                {
                    notification = new BuildNotification(planKey, BuildStatus.Resting, "Nothing to worry about. Now get back to work!");
                }
            }

            var notifier = new CompositeBuildNotifier(new GrowlBuildNotifier(), new ArduinoBuildNotifier());

            var cacheFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"PreviousBambooPlanReading.cache");
            var reading = GetPreviousBambooPlanReading(cacheFile);

            if (reading != null && reading.PlanKey == planKey && DateTime.UtcNow.Subtract(reading.LastRunUtc) < TimeSpan.FromSeconds(70))
            {
                if (reading.BuildStatus != (int) notification.Status)
                {
                    notifier.Notify(notification);
                }
            }
            else
            {
                notifier.Notify(notification);
            }

            UpdateBambooPlanReading(cacheFile, notification, planKey);
        }

        private static void UpdateBambooPlanReading(string cacheFile, BuildNotification notification, string planKey)
        {
            var newReading = new BambooPlanReading(DateTime.UtcNow, (int) notification.Status, planKey);
            File.WriteAllText(cacheFile, JsonConvert.SerializeObject(newReading));
        }

        private static BambooPlanReading GetPreviousBambooPlanReading(string cacheFile)
        {
            if (!File.Exists(cacheFile))
            {
                var writer = File.CreateText(cacheFile);
                writer.Close();
            }
            var cacheStr = File.ReadAllText(cacheFile);
            var reading = JsonConvert.DeserializeObject<BambooPlanReading>(cacheStr);
            return reading;
        }
    }
}
