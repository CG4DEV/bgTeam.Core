namespace bgTeam.StoryRunner.Domain
{
    using System;
    using System.Reflection;

    public class StoryInfo
    {
        public string ContextName { get { return ContextType != null ? ContextType.Name : string.Empty; } }

        public string StoryName { get; set; }

        public Type StoryType { get; set; }

        public Type ContextType { get; set; }

        public MethodInfo ExecuteMethodInfo { get; set; }
    }
}
