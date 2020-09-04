namespace bgTeam.StoryRunnerScheduler
{
    using System;
    using bgTeam.Quartz;

    public class JobTriggerInfo : IJobTriggerInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DateStart { get; set; }

        public string NameQueue { get; set; }

        public string ContextType { get; set; }

        public dynamic[] ContextValue { get; set; }

        public int? DateChangeOffsetFrom { get; set; }

        public int? DateChangeOffsetTo { get; set; }

        public string[] Sql { get; set; }

        public string SqlString => Sql != null ? string.Join(Environment.NewLine, Sql) : string.Empty;
    }
}
