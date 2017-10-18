namespace bgTeam.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IWebClient
    {
        Task<T> GetAsync<T>(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
            where T : class;

        Task<T> PostAsync<T>(string method, object postParams)
            where T : class;
    }
}
