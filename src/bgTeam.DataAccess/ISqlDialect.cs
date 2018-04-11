namespace bgTeam.DataAccess
{
    public interface ISqlDialect
    {
        void Init(SqlDialectEnum dialect);
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
