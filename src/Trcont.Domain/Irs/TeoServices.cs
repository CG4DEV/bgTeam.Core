namespace Trcont.Domain.Irs
{
    using System;
    using Trcont.Domain.Entity;

    public class TeoServices : KpServices
    {
        public Guid? ParamDangerGuid { get; set; }

        public Guid? ParamSendReceiveGuid { get; set; }

        public Guid? ParamComplexTypeGuid { get; set; }

        public Guid? ParamTerminalGuid { get; set; }

        public Guid? ParamWeighignGuid { get; set; }

        public Guid? ParamAddEquipmentTypeGuid { get; set; }

        public Guid? ParamTreamentCargoTypeGuid { get; set; }

        public Guid? ParamWorkTypeGuid { get; set; }

        public Guid? ParamSvcAddCondGuid { get; set; }

        public Guid? ParamOutCatGuid { get; set; }
    }
}
