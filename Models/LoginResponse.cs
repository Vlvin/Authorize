

namespace Authorize.Models;

public class LoginResponse
{
    public required string Token { get; set; }
    public required DateTime Expiration { get; set; }

}