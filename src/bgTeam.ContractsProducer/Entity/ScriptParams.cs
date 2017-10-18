namespace bgTeam.ContractsProducer.Entity
{
    using DapperExtensions.Attributes;
    using System;

    [TableName("RefTemplateParams")]
    public class ScriptParams
    {
        public int ReferenceId { get; set; }

        public int ParamId { get; set; }

        public string ParamTitle { get; set; }

        public string ParamName { get; set; }

        public int ParamType { get; set; }

        public int MaxLen { get; set; }

        public int ReadOnly { get; set; }

        public int Visible { get; set; }

        public string DefValue1 { get; set; }

        public string DefValue2 { get; set; }

        public int? ParamInt1 { get; set; }

        public int? ParamInt2 { get; set; }

        public string ParamStr1 { get; set; }

        public DateTime? ParamDateTime1 { get; set; }

        public float? ParamFloat1 { get; set; }
    }
}
