namespace bgTeam.Impl.Quartz
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IJobTriggerInfo
    {
        string Name { get; set; }

        string DateStart { get; set; }
    }
}
