using Microsoft.AspNetCore.Identity;

namespace Domain.User;

public class UserDomain : IdentityUser
{
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; } = string.Empty;

    public void Update(string personalIdentifier, string userName, string email, bool emailConfirmed)
    {
        throw new NotImplementedException();
    }
}