namespace bgTeam.DataAccess
{
    public interface ISqlDialect
    {
        void Init(SqlDialectEnum dialect);

        ISqlObject GeneratePagingSql(string query, int page, int pageSize, object parameters = null);
    }
}
