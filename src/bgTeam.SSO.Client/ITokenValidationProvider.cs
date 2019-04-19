namespace bgTeam.SSO.Client
{
    using System.Threading.Tasks;

    public interface ITokenValidationProvider
    {
        Task<bool> ValidateTokenAsync(string token);
    }
}
