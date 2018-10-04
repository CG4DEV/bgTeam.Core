namespace bgTeam.DataAccess.Impl.MsSql
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.SqlServer.Server;

    public abstract class IntDbType
    {
        private readonly IEnumerable<int> _ids;

        public IntDbType(IEnumerable<int> ids)
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
            var numberList = new List<SqlDataRecord>();

            var tvpDefinition = new[] { new SqlMetaData("id", SqlDbType.Int) };

            foreach (var id in _ids)
            {
                var rec = new SqlDataRecord(tvpDefinition);
                rec.SetInt32(0, id);
                numberList.Add(rec);
            }

            return new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.Structured,
                Direction = ParameterDirection.Input,
                TypeName = "IntegerIdList",
                Value = numberList,
            };
        }
    }
}
