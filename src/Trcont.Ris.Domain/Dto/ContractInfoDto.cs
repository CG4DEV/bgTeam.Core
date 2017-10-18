namespace Trcont.Ris.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using bgTeam.Extensions;
    using Trcont.Ris.Domain.Entity;

    public class ContractInfoDto
    {
        public int Id { get; set; }

        public Guid ContractGuid { get; set; }

        public string ContractNumber { get; set; }

        public string ContractTitle { get; set; }

        public StatusContractEnum Status { get; set; }

        public string StatusStr => Status.GetDescription();

        public DateTime ContractDate { get; set; }

        public DateTime ContractBeginDate { get; set; }

        public DateTime ContractEndDate { get; set; }

        public Guid ClientGuid { get; set; }

        public ClientTypeEnum ClientType { get; set; }

        public string ClientTypeStr => ClientType.GetDescription();

        public string ClientName { get; set; }

        public AccTypeContractEnum AccType { get; set; }

        public string AccTypeStr => AccType.GetDescription();

        public string BIK { get; set; }

        public string BankTitle { get; set; }

        public string BankAddress { get; set; }

        public string JIndex { get; set; }

        public string JCity { get; set; }

        public string JAddress { get; set; }

        public string FIndex { get; set; }

        public string FCity { get; set; }

        public string FAddress { get; set; }

        public string Inn { get; set; }

        public string Kpp { get; set; }

        public decimal Summ { get; set; }

        public IEnumerable<ContractParam> Params { get; set; }
    }

    public enum ClientTypeEnum
    {
        [Description("Юридическое лицо")]
        Entity = 0,

        [Description("Физическое лицо")]
        Individual = 1,
    }

    public enum StatusContractEnum
    {
        [Description("Не задано")]
        None = 0,

        [Description("На подписании")]
        Signing = 1,

        [Description("Заключён")]
        Imprisoned = 2,

        [Description("Отклонён")]
        Reject = 3,
    }

    public enum AccTypeContractEnum
    {
        [Description("Рублёвый")]
        Rub = 0,

        [Description("Валютный")]
        Monetary = 1,
    }
}
