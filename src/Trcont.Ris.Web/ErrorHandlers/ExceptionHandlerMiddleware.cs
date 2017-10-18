namespace Trcont.Ris.Web.ErrorHandlers
{
    using System;
    using System.Threading.Tasks;
    using bgTeam;
    using Microsoft.AspNetCore.Http;

    public sealed class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLogger _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, IAppLogger logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                throw;
            }
        }
    }
}
