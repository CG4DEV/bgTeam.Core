namespace bgTeam.SSO.Client
{
    using System.Threading.Tasks;

    public sealed class LocalTokenValidationProvider : ITokenValidationProvider
    {
        public LocalTokenValidationProvider()
        {
        }

        // TODO: make this method to verify signature of the token by utilizing public key without any remote calls (requires asymmetric cipher implementation in the SSO service itself)
        public async Task<bool> ValidateTokenAsync(string token)
        {
            return await Task.FromResult(true);
        }
    }
}
