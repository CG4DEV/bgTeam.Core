namespace Trcont.Ris.Story.Common
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;

    public class GetDictionaryStory : IStory<GetDictionaryStoryContext, IEnumerable<object>>
    {
        private static readonly int _selectTop = 50;

        private static readonly Dictionary<string, string> _queries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Zone", $"SELECT TOP {_selectTop} z.Id, z.IrsGuid AS Guid, z.Name FROM Zone z WITH(NOLOCK) [WHERE z.Name LIKE @Search]" },
            { "Client", $"SELECT TOP {_selectTop} c.Id, c.ClientGUID AS Guid, c.ClientName AS Name FROM vClients c WITH(NOLOCK) [WHERE c.ClientName LIKE @Search] ORDER BY c.ClientName" },
            { "Point", $"SELECT TOP {_selectTop} p.Id, p.IrsGuid AS Guid, p.Title AS Name FROM vPoints p WITH(NOLOCK) [WHERE p.Title LIKE @Search]" },
            { "Unit", $"SELECT TOP {_selectTop} u.Id, u.IrsGuid AS Guid, u.Title AS Name FROM Unit u WITH(NOLOCK) [WHERE u.Title LIKE @Search]" }
        };

        private readonly IRepository _repository;

        public GetDictionaryStory(IRepository repository)
        {
            _repository = repository;
        }

        public static Dictionary<string, string> Queries => _queries;

        public IEnumerable<object> Execute(GetDictionaryStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<object>> ExecuteAsync(GetDictionaryStoryContext context)
        {
            string query;
            if (_queries.TryGetValue(context.Name, out query))
            {
                IEnumerable<ResultRequst> res;
                if (string.IsNullOrEmpty(context.Search))
                {
                    res = await _repository.GetAllAsync<ResultRequst>(PrepareQuery(query));
                }
                else
                {
                    if (context.Search.Length >= 3)
                    {
                        res = await _repository.GetAllAsync<ResultRequst>(PrepareQuery(query, true), new { Search = $"%{context.Search}%" });
                    }
                    else
                    {
                        return null;
                    }
                }

                // TODO : Убираем дубли, мега костыль
                /*var list = new List<ResultRequst>();
                var groupList = res.GroupBy(x => x.Name);
                foreach (var item in groupList)
                {
                    var row = item.First();
                    list.Add(row);
                }*/

                return res.DistinctBy(x => x.Name);
            }

            throw new KeyNotFoundException($"Справочник {context.Name} не найден");
        }

        private string PrepareQuery(string query, bool search = false)
        {
            if (!search)
            {
                return Regex.Replace(query, @"\[\D+\]", string.Empty);
            }
            else
            {
                return Regex.Replace(query, @"(\[|\])", string.Empty);
            }
        }

        class ResultRequst
        {
            public int Id { get; set; }

            public Guid Guid { get; set; }

            public string Name { get; set; }
        }
    }
}
