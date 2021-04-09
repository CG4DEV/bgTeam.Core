﻿namespace bgTeam.SSO.Client
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public static class SsoExtensions
    {
        public static void AddSsoAuthentication(
            this IServiceCollection services,
            ITokenValidationProvider tokenValidationProvider,
            Action onSuccess = null,
            Action onFail = null)
        {
            if (tokenValidationProvider == null)
            {
                throw new ArgumentNullException(nameof(tokenValidationProvider));
            }

            services.AddAuthentication(Constants.SCHEME_NAME)
                .AddScheme<SsoAuthenticationOptions, SsoAuthenticationHandler>(
                    Constants.SCHEME_NAME,
                    opt =>
                    {
                        opt.TokenValidationProvider = tokenValidationProvider;
                        opt.OnSuccess = onSuccess;
                        opt.OnFail = onFail;
                    });
        }
    }
}