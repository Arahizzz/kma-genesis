using System.Net;

namespace ApiService;

public class HttpResponseException : Exception
{
    public HttpStatusCode StatusCode { get; set; }

    public HttpResponseException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }
}