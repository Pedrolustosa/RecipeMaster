using System.Net;

namespace RecipeMaster.API.Exceptions;

public class InternalServerException : BaseException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    public InternalServerException(string message, string? details = null)
        : base(message, details) { }
}
