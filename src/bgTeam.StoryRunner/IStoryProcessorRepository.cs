namespace bgTeam.StoryRunner
{
    public interface IStoryProcessorRepository
    {
        StoryInfo Get(string contextName);
    }
}
