namespace bgTeam.StoryRunner
{
    using System.Threading.Tasks;
    using bgTeam.Queues;

    public interface IStoryProcessor
    {
        void Process(StoryRunnerMessageWork info);

        Task ProcessAsync(StoryRunnerMessageWork info);
    }
}
