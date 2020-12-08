namespace bgTeam.DataAccess.Impl.Dapper
{
    using global::Dapper;

    public class SqlDialectWithUnderscoresDapper : SqlDialectDapper
    {
        public override void Init(SqlDialectEnum dialect)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            base.Init(dialect);
        }
    }
}
