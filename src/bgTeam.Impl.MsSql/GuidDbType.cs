namespace DapperExtensions.Mapper
{
    using Microsoft.SqlServer.Server;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class GuidDbType
    {
        private readonly IEnumerable<Guid> _ids;

        public GuidDbType(IEnumerable<Guid> ids)
        {
            _ids = ids;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            var sqlCommand = (SqlCommand)command;
            sqlCommand.Parameters.Add(GetParameter(name));
        }

        public SqlParameter GetParameter(string name)
        {
            var guidList = new List<SqlDataRecord>();

            var tvpDefinition = new[] { new SqlMetaData("id", SqlDbType.UniqueIdentifier) };

            foreach (var id in _ids)
            {
                var rec = new SqlDataRecord(tvpDefinition);
                rec.SetSqlGuid(0, id);
                guidList.Add(rec);
            }

            return new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.Structured,
                Direction = ParameterDirection.Input,
                TypeName = "GuidIdList",
                Value = guidList
            };
        }
    }
}
