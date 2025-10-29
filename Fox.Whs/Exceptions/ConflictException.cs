namespace Fox.Whs.Exceptions;

/// <summary>
/// Exception cho lỗi xung đột dữ liệu (409 Conflict)
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
