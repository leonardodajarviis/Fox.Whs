namespace Fox.Whs.Exceptions;

/// <summary>
/// Exception cho lỗi không tìm thấy tài nguyên (404 Not Found)
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NotFoundException(string resourceName, object key) 
        : base($"{resourceName} với id '{key}' không tồn tại")
    {
    }
}
