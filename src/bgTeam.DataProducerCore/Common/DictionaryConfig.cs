namespace bgTeam.DataProducerCore.Common
{
    using bgTeam.ProduceMessages;

    public class DictionaryConfig : IDictionaryConfig
    {
        public string ConfigName { get; set; }

        public string EntityType { get; set; }

        public string EntityKey { get; set; }

        public string Description { get; set; }

        public string DateFormatStart { get; set; }

        public int? DateChangeOffset { get; set; }

        public int? GroupOrderBy { get; set; }

        public string GroupName { get; set; }

        public int Pool { get; set; }

        public string[] Sql { get; set; }

        public string SqlString => Sql != null ? string.Join("\r\n", Sql) : string.Empty;
    }
}
