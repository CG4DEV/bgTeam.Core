namespace bgTeam.ProcessMessages
{
    using bgTeam.Queues;

    public interface IEntityMapService
    {
        /// <summary>
        /// Конвертирует сообзение из очереди в объект EntityMap.
        /// </summary>
        /// <param name="message">Сообщение из очереди.</param>
        /// <returns></returns>
        EntityMap CreateEntityMap(IQueueMessage message);
    }
}
