namespace bgTeam.SSO.Client
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    // TODO: cache responses: public key -> response
    // TODO: http client issues review: http://www.nimaara.com/2016/11/01/beware-of-the-net-httpclient/
    // TODO: add some client-side check. Don't call the remote service unless the client-side checks were passed
    public class SsoClient
    {
        public async Task<bool> ValidateToken(string token)
        {
            const string validateUrl = "api/auth/validatetoken";

            var address = $"https://{GetIssuerAddressFromToken(token)}/{validateUrl}";
            SsoReplyResult reply = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var res = await httpClient.GetStringAsync(new Uri(address));
                reply = JsonConvert.DeserializeObject<SsoReplyResult>(res);
            }

            return reply.Succeeded;
        }

        private string GetIssuerAddressFromToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);

            return jwtToken.Issuer;
        }

        private class SsoReplyResult
        {
            public bool Succeeded { get; set; }
        }
    }
}
