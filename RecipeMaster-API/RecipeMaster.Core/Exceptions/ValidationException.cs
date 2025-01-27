using System.Net;

namespace RecipeMaster.Core.Exceptions;

public class ValidationException(IDictionary<string, string[]> errors) : BaseException("Validation failed.")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;
    public IDictionary<string, string[]> Errors { get; } = errors;
}
