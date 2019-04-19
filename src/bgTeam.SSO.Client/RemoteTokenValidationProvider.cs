namespace bgTeam.SSO.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure;
    using Newtonsoft.Json;

    public sealed class RemoteTokenValidationProvider : ITokenValidationProvider
    {
        private readonly string _ssoServiceEndpoint;

        public RemoteTokenValidationProvider(string ssoServiceEndpoint)
        {
            _ssoServiceEndpoint = ssoServiceEndpoint;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            SsoReplyResult reply = null;

            var defaultHeaders = new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {token}"}
            };

            using (var client = new RestClient(defaultHeaders))
            {
                var res = await client.GetStringAsync(new Uri(_ssoServiceEndpoint));
                reply = JsonConvert.DeserializeObject<SsoReplyResult>(res);
            }

            return reply.Succeeded;
        }

        private class SsoReplyResult
        {
            public bool Succeeded { get; set; }
        }
    }
}
