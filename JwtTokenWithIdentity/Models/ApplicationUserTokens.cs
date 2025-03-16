using Microsoft.AspNetCore.Identity;

namespace JwtTokenWithIdentity.Models
{
    public class ApplicationUserTokens : IdentityUserToken<string>
    {
        public DateTime ExpireDate { get; set; }
    }
}
