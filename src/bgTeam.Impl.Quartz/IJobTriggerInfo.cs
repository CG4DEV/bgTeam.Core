namespace bgTeam.Quartz
{
    public interface IJobTriggerInfo
    {
        string Name { get; set; }

        // May be should be named CronString ?

        /// <summary>
        /// Examples and format of cron expressions <see href="https://www.freeformatter.com/cron-expression-generator-quartz.html">here</see>
        /// </summary>
        string DateStart { get; set; }
    }
}
