using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenWithIdentity.Models
{
    public class Products
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        [DisplayName("Adı")]
        public string Name { get; set; }

        [Required]
        [StringLength(60)]
        [DisplayName("Birim fiyat")]
        public float UnitPrice { get; set; }
    }
}
