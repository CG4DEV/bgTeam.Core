namespace Trcont.Ris.Domain.Dto
{
    using System;

    public class FactObjectDto
    {
        public bool AutoFromDone { get; set; }

        public bool AutoToDone { get; set; }

        public MainState MainStarted { get; set; }

        public MainState MainDone { get; set; }
    }

    public enum MainState
    {
        NotStarted = 0,
        InTransit = 1,
        Arrive = 2
    }
}
