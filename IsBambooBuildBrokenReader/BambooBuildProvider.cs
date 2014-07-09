namespace IsBambooBuildBrokenReader
{
    public class BambooBuildProvider : IBuildProvider
    {
        private readonly string _planKey;
        private readonly string _bambooLatestRestApiUri;

        public BambooBuildProvider(string planKey, string bambooLatestRestApiUri)
        {
            _planKey = planKey;
            _bambooLatestRestApiUri = bambooLatestRestApiUri;
        }

        public BuildNotification GetLatestBuildNotification()
        {
            using (var bamboo = new Bamboo(_bambooLatestRestApiUri))
            {
                var plan = bamboo.GetPlan(_planKey);

                if (plan.IsBuilding)
                {
                    return new BuildNotification(_planKey, BuildStatus.Building, "Red 5 standing by");
                }

                var result = bamboo.GetLatestResultForPlan(_planKey);

                if (!result.WasSuccessful())
                {
                    return new BuildNotification(_planKey, BuildStatus.Broken, "Success! Row inserted");
                }

                return new BuildNotification(_planKey, BuildStatus.Resting,
                    "Nothing to worry about. Now get back to work!");
            }
        }
    }
}