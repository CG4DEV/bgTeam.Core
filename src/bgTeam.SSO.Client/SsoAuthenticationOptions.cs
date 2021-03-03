namespace bgTeam.SSO.Client
{
    using System;
    using Microsoft.AspNetCore.Authentication;

    internal class SsoAuthenticationOptions : AuthenticationSchemeOptions
    {
        public SsoAuthenticationOptions()
        {
            ClaimsIssuer = Constants.SCHEME_NAME;
        }

        public ITokenValidationProvider TokenValidationProvider { get; set; }

        public Action OnSuccess { get; set; }

        public Action OnFail { get; set; }
    }
}
