namespace Fox.Whs.Exceptions;

/// <summary>
/// Exception cho lỗi không có quyền truy cập (401 Unauthorized)
/// </summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public UnauthorizedException() : base("Bạn không có quyền truy cập")
    {
    }
}
