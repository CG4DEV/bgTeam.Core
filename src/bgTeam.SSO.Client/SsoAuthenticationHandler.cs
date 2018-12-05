namespace bgTeam.SSO.Client
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    internal class SsoAuthenticationHandler : AuthenticationHandler<SsoAuthenticationOptions>
    {
        public SsoAuthenticationHandler(IOptionsMonitor<SsoAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
          : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!TryGetTokenFromRequest(Request, out var token))
            {
                return AuthenticateResult.NoResult();
            }

            try
            {
                if (await Options.ValidateTokenFunc(token))
                {
                    return Success(token);
                }

                // _log.Info("Token verification failed", Array.Empty<object>());
                return AuthenticateResult.NoResult();
            }
            catch (Exception ex)
            {
                // log
                return AuthenticateResult.NoResult();
            }
        }

        private AuthenticateResult Success(string token)
        {
            return AuthenticateResult.Success(new AuthenticationTicket(DecodeToken(token), new AuthenticationProperties(), Constants.SCHEME_NAME));
        }

        private ClaimsPrincipal DecodeToken(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, Constants.SCHEME_NAME));
        }

        private bool TryGetTokenFromRequest(HttpRequest request, out string token)
        {
            var key = "Authorization";
            if (request.Headers.ContainsKey(key))
            {
                string[] strArray = request.Headers[key].First().Split(' ');
                if (strArray.Length == 2)
                {
                    token = strArray[1];
                    return true;
                }
            }

            token = null;
            return false;
        }
    }
}
