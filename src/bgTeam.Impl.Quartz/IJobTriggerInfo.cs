namespace bgTeam.Quartz
{
    public interface IJobTriggerInfo
    {
        string Name { get; set; }

        string DateStart { get; set; }
    }
}
