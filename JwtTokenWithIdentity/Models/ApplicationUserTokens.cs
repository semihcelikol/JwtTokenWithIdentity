using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenWithIdentity.Models
{
    public class ApplicationUserTokens : IdentityUserToken<string>
    {
        public DateTime ExpireDate { get; set; }
    }
}
