namespace bgTeam.ProcessMessages
{
    using bgTeam.Queues;
    using System.Threading.Tasks;

    public interface IMessageProcessor
    {
        IQuery Process(IQueueMessage message);

        Task<IQuery> ProcessAsync(IQueueMessage message);
    }
}
