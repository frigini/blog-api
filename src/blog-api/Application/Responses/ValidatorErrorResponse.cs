namespace BlogApi.Application.Responses;

public class ValidationErrorResponse(List<ValidationError> errors)
{
    public List<ValidationError> Errors { get; } = errors;
}

public class ValidationError(string property, string message)
{
    public string Property { get; } = property;
    public string Message { get; } = message;
}
