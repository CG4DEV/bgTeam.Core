namespace bgTeam.Web
{
    using System.Net.Http;

    public interface IContentBuilder
    {
        HttpContent Build(object param);
    }
}
