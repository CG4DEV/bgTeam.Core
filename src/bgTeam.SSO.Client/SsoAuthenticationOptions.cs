namespace bgTeam.SSO.Client
{
    using Microsoft.AspNetCore.Authentication;
    using System;

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
