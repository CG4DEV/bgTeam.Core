namespace bgTeam.Queues
{
    public class QueueMessageWork
    {
        public ulong Id { get; set; }

        public string Body { get; set; }
    }

    ////public class QueueProvider : IQueueProvider
    ////{
    ////    public QueueMessageWork AskMessage(string queueName)
    ////    {
    ////        //var queue = _сlientFactory
    ////        //    .Create()
    ////        //    .Queue(criterion.QueueName);

    ////        //var message = queue.Next(criterion.ReservationSeconds);
    ////        //if (message == null)
    ////        //    return null;

    ////        //try
    ////        //{
    ////        //    var entity = _messageProvider.ExtractObject<TEntity>(message.Body);
    ////        //    return new MqEntityContainer<TEntity>(message.Id, message.ReservationId, entity);
    ////        //}
    ////        //catch (Exception exception)
    ////        //{
    ////        //    throw new DeserializeMessageException(message.Id, message.ReservationId, "Message Deserialization problem", exception);
    ////        //}
    ////        return null;
    ////    }

    ////    public void DeleteMessage<TEntity>(object message)
    ////    {
    ////        throw new NotImplementedException();
    ////    }
    ////}

    ////public class MqQuery<TEntity> : IQuery<IQueueCriterion, IMqEntityContainer<TEntity>>
    ////{
    ////    private readonly IMessageProvider _messageProvider;

    ////    private readonly IMqClientFactory _сlientFactory;

    ////    public MqQuery(IMqClientFactory clientFactory, IMessageProvider messageProvider)
    ////    {
    ////        _messageProvider = messageProvider;
    ////        _сlientFactory = clientFactory;
    ////    }

    ////    public IMqEntityContainer<TEntity> Ask(IQueueCriterion criterion)
    ////    {
    ////        var queue = _сlientFactory
    ////            .Create()
    ////            .Queue(criterion.QueueName);

    ////        var message = queue.Next(criterion.ReservationSeconds);
    ////        if (message == null)
    ////            return null;

    ////        try
    ////        {
    ////            var entity = _messageProvider.ExtractObject<TEntity>(message.Body);
    ////            return new MqEntityContainer<TEntity>(message.Id, message.ReservationId, entity);
    ////        }
    ////        catch (Exception exception)
    ////        {
    ////            throw new DeserializeMessageException(message.Id, message.ReservationId, "Message Deserialization problem", exception);
    ////        }
    ////    }
    ////}
}