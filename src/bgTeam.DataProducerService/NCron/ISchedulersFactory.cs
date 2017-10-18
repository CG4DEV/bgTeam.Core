namespace bgTeam.DataProducerService.NCron
{
    using bgTeam.DataProducerCore.Common;
    using Quartz;
    using System.Collections.Generic;

    public interface ISchedulersFactory
    {
        IScheduler Create(DictionaryConfig config);

        IScheduler CreateGroup(IEnumerable<DictionaryConfig> configs);

        IEnumerable<IScheduler> GetAllSchedulers();
    }
}
