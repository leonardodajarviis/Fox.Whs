namespace Fox.Whs.Dtos;

public class PaginationResponse<T>
{
    public List<T> Results { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPage => (int)Math.Ceiling((double)TotalCount / PageSize);
}
