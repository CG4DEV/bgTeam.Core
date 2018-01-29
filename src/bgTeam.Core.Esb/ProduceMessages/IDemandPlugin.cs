namespace bgTeam.Esb.ProduceMessages
{
    using bgTeam.ProduceMessages;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDemandPlugin : IDictionaryConfig
    {
        string ConfigName { get; }

        IEnumerable<dynamic> Execute(Dictionary<string, object> map);

        Task<IEnumerable<dynamic>> ExecuteAsync(Dictionary<string, object> map);
    }
}
