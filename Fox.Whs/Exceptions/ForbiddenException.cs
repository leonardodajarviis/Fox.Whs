namespace Fox.Whs.Exceptions;

/// <summary>
/// Exception cho lỗi bị cấm truy cập (403 Forbidden)
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ForbiddenException() : base("Bạn không có quyền thực hiện hành động này")
    {
    }
}
