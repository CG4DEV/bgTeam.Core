namespace bgTeam.Web.Exceptions
{
    using System;
    using System.Net;
    using System.Net.Http;

    public class WebClientException : HttpRequestException
    {
        public WebClientException(HttpStatusCode statusCode)
            : this(null, statusCode)
        {
        }

        public WebClientException(string message, HttpStatusCode statusCode)
            : this(message, null, statusCode)
        {
        }

        public WebClientException(string message, Exception inner, HttpStatusCode statusCode)
            : base(message, inner)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; private set; }
    }
}
