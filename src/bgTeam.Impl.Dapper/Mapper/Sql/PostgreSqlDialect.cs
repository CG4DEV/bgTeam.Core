namespace DapperExtensions.Mapper.Sql
{
    using System.Collections.Generic;

    public class PostgreSqlDialect : SqlDialectBase
    {
        public override string GetIdentitySql(string tableName)
        {
            return "SELECT LASTVAL() AS Id";
        }

        public override string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            int startValue = page * resultsPerPage;
            return GetSetSql(sql, startValue, resultsPerPage, parameters);
        }

        public override string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters)
        {
            string result = string.Format("{0} LIMIT @maxResults OFFSET @pageStartRowNbr", sql);
            parameters.Add("@maxResults", maxResults);
            parameters.Add("@pageStartRowNbr", firstResult /* * maxResults */); //HACK: fixing DapperExtensions bug
            return result;
        }
    }
}
