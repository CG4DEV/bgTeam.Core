namespace bgTeam.StoryRunner.Common
{
    using bgTeam.StoryRunner.Domain;

    public interface IStoryProcessorRepository
    {
        StoryInfo Get(string contextName);
    }
}
