namespace Trcont.OrderRoutesCreator.Common
{
    public class QuartzConfig
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DateFormatStart { get; set; }

        public int? DateChangeOffset { get; set; }

        public int Pool { get; set; }

        public string[] Sql { get; set; }

        public string SqlString => Sql != null ? string.Join("\r\n", Sql) : string.Empty;
    }
}
