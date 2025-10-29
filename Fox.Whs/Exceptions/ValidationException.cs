namespace Fox.Whs.Exceptions;

/// <summary>
/// Exception cho lỗi validation (422 Unprocessable Entity)
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Danh sách lỗi validation theo field
    /// </summary>
    public IDictionary<string, string[]>? Errors { get; }

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, IDictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }

    public ValidationException(IDictionary<string, string[]> errors) 
        : base("Một hoặc nhiều lỗi validation xảy ra")
    {
        Errors = errors;
    }
}
