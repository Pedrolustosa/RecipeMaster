using System.Net;

namespace RecipeMaster.Core.Exceptions;

public class InternalServerException(string message, string? details = null) : BaseException(message, details)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
}
