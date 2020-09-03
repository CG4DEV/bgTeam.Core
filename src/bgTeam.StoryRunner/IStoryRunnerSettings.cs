namespace bgTeam.StoryRunner
{
    public interface IStoryRunnerSettings
    {
        string StoryQueue { get; }

        string ErrorQueue { get; }
    }
}
