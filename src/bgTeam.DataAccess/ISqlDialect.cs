namespace bgTeam.DataAccess
{
    public interface ISqlDialect
    {
        void Init(SqlDialectEnum dialect);

        string GeneratePagingSql(string query, int page, int pageSize);
    }

    public enum SqlDialectEnum
    {
        MsSql,
        Oracle,
        PostgreSql,
        MySql,
        Sqlite
    }
}
