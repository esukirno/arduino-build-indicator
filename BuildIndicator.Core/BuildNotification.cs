namespace BuildIndicator.Core
{
    public class BuildNotification
    {
        public BuildNotification(ResultCheckpoint checkpoint, string name, string description, BuildStatus status, string triggeredBy)
        {
            Checkpoint = checkpoint;
            Name = name;
            Description = description;
            Status = status;
            TriggeredBy = triggeredBy;
        }

        public ResultCheckpoint Checkpoint { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BuildStatus Status { get; set; }
        public string StatusAsString { get { return Status.ToString(); } }
        public string TriggeredBy { get; set; }

        public override string ToString()
        {
            return string.Format("Checkpoint: {0}, Name: {1}, Status: {2}", Checkpoint, Name, Status);
        }
    }
}