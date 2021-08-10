using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenWithIdentity.Models
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
