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

        public WebClientException(string message, HttpStatusCode statusCode, HttpResponseMessage response)
            : this(message, null, statusCode, response)
        {
        }

        public WebClientException(string message, Exception inner, HttpStatusCode statusCode)
            : this(message, inner, statusCode, null)
        {
        }

        public WebClientException(string message, Exception inner, HttpStatusCode statusCode, HttpResponseMessage response)
            : base(message, inner)
        {
            StatusCode = statusCode;
            OuterMessage = response;
        }

        public HttpStatusCode StatusCode { get; private set; }

        public HttpResponseMessage OuterMessage { get; private set; }
    }
}
