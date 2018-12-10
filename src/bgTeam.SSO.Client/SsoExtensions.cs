namespace bgTeam.SSO.Client
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public static class SsoExtensions
    {
        public static void AddSsoAuthentication(this IServiceCollection services, Func<string, Task<bool>> validateTokenFunc)
        {
            services.AddAuthentication(Constants.SCHEME_NAME)
                .AddScheme<SsoAuthenticationOptions, SsoAuthenticationHandler>(Constants.SCHEME_NAME,
                    opt => opt.ValidateTokenFunc = validateTokenFunc);
        }
    }
}
