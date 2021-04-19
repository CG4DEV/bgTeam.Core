namespace bgTeam.StoryRunner
{
    using System.Threading.Tasks;
    using bgTeam.Queues;

    public interface IStoryProcessor
    {
        void Process(StoryRunnerMessageWork info);

        object ProcessWithResult(StoryRunnerMessageWork info);

        Task ProcessAsync(StoryRunnerMessageWork info);

        Task<object> ProcessWithResultAsync(StoryRunnerMessageWork info);
    }
}
