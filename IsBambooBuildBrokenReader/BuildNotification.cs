namespace IsBambooBuildBrokenReader
{
    public class BuildNotification
    {
        public BuildNotification(string name, BuildStatus status, string description)
        {
            Name = name;
            Status = status;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public BuildStatus Status { get; set; }
    }
}