namespace bgTeam.ProduceMessages
{
    using System.Collections.Generic;

    public interface IPluginSchedulersFactory
    {
        string EntityType { get; }

        string GroupName { get; }

        void Create(IDictionaryConfig config);

        void CreateGroup(IEnumerable<IDictionaryConfig> configs);
    }
}
