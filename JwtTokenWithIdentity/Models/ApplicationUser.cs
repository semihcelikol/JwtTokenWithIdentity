using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenWithIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DisplayName("Ad soyad")]
        [StringLength(60)]
        public string FullName { get; set; }
    }
}
