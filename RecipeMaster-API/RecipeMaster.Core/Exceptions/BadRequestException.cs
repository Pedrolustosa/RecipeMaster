using System.Net;

namespace RecipeMaster.Core.Exceptions;

public class BadRequestException(string message) : BaseException(message)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
