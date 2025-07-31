using Microsoft.AspNetCore.Identity;

namespace Domain.User;

public class UserDomain : IdentityUser
{
    public string? PersonalIdentifier { get; set; }

    public void Update(string personalIdentifier, string userName, string email, bool emailConfirmed)
    {
        throw new NotImplementedException();
    }
}