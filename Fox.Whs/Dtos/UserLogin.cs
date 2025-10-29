namespace Fox.Whs.Dtos;

public record UserLogin
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}