namespace bgTeam.SSO.Client
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;

    public class SsoAuthenticationOptions : AuthenticationSchemeOptions
    {
        public SsoAuthenticationOptions()
        {
            ClaimsIssuer = Constants.SCHEME_NAME;
        }

        public Func<string, Task<bool>> ValidateTokenFunc { get; set; }
    }
}
