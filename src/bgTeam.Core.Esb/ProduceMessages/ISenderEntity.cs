namespace bgTeam.ProduceMessages
{
    public interface ISenderEntity
    {
        void Send(object entity, string entityKey);
    }
}
