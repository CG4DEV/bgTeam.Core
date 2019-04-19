namespace bgTeam.SSO.Client
{
    using Microsoft.AspNetCore.Authentication;

    internal class SsoAuthenticationOptions : AuthenticationSchemeOptions
    {
        public SsoAuthenticationOptions()
        {
            ClaimsIssuer = Constants.SCHEME_NAME;
        }

        public ITokenValidationProvider TokenValidationProvider { get; set; }
    }
}
