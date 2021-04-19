namespace bgTeam.StoryRunner
{
    using System.Threading.Tasks;
    using bgTeam.Queues;

    public interface IStoryProcessor
    {
        Task ProcessAsync(StoryRunnerMessageWork info);

        Task<object> ProcessWithResultAsync(StoryRunnerMessageWork info);
    }
}
