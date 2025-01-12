using System.Net;

namespace RecipeMaster.API.Exceptions;

public class NotFoundException : BaseException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    public NotFoundException(string resource, object key)
        : base($"The resource '{resource}' with key '{key}' was not found.") { }
}
