namespace bgTeam.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISqlObject
    {
        /// <summary>
        /// Sql string
        /// </summary>
        string Sql { get; }

        /// <summary>
        /// Parameter list
        /// </summary>
        object QueryParams { get; }
    }
}
