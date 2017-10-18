namespace bgTeam.ProduceMessages
{
    public interface IDictionaryConfig
    {
        string EntityType { get; set; }

        string EntityKey { get; set; }

        string Description { get; set; }

        string DateFormatStart { get; set; }

        int? DateChangeOffset { get; set; }

        int? GroupOrderBy { get; set; }

        string GroupName { get; set; }

        int Pool { get; set; }

        string SqlString { get; }
    }
}
