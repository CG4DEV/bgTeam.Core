namespace bgTeam.ContractsProducer.Dto
{
    using bgTeam.ContractsProducer.Entity;
    using System.Collections.Generic;

    public class ScriptTemplate
    {
        public IEnumerable<ScriptStepDto> TemplateInfo { get; set; }

        public IEnumerable<ScriptParams> ScriptParams { get; set; }
    }
}
