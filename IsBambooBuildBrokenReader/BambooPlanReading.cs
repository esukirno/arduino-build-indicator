using System;

namespace IsBambooBuildBrokenReader
{
    public class BambooPlanReading
    {
        public BambooPlanReading(DateTime lastRunUtc, int buildStatus, string planKey)
        {
            LastRunUtc = lastRunUtc;
            BuildStatus = buildStatus;
            PlanKey = planKey;
        }

        public DateTime LastRunUtc { get; set; }
        public string PlanKey { get; set; }
        public int BuildStatus { get; set; }
    }
}