namespace bgTeam.ContractsQueryFactory.Dto
{
    public class SaldoServiceInfoDto
    {
        public int ServiceId { get; set; }

        public string ServiceTitle { get; set; }

        public int PayAccountId { get; set; }

        public string PayAccountTitle { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyTitle { get; set; }

        public int Forbidden { get; set; }

        public decimal Saldo { get; set; }

        public decimal Summa { get; set; }

        public int Index { get; set; }
    }
}
