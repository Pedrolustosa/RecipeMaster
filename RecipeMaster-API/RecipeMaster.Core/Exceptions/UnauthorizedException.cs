using System.Net;

namespace RecipeMaster.Core.Exceptions;

public class UnauthorizedException(string message) : BaseException(message)
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
