using System.Net;

namespace RecipeMaster.API.Exceptions;

public class ValidationException : BaseException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("Validation failed.")
    {
        Errors = errors;
    }
}
