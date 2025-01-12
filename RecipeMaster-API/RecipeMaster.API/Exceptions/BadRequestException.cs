using System.Net;

namespace RecipeMaster.API.Exceptions;

public class BadRequestException : BaseException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public BadRequestException(string message)
        : base(message) { }
}
