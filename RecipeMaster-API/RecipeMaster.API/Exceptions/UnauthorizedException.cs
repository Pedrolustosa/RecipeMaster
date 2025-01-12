using System.Net;

namespace RecipeMaster.API.Exceptions;

public class UnauthorizedException : BaseException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

    public UnauthorizedException(string message) : base(message) { }
}
