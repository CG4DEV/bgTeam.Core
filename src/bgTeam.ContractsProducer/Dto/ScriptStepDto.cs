namespace bgTeam.ContractsProducer.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ScriptStepDto
    {
        public int DSScriptStepId { get; set; }

        public int PosInd { get; set; }

        public int StepType { get; set; }

        public string Caption { get; set; }

        public int BeginTrans { get; set; }

        public int CommitTrans { get; set; }

        public int RollBackTrans { get; set; }

        public byte[] Content { get; set; }
    }
}
