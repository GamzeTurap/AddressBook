using AddressBookEL.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookEL.Models
{
    [Table("UserAddresses")] //Bu nedenle tablolara es takisi vermeden kullaniyorlar
    public class UserAddress:BaseNumeric
    {
        [Required]
        [StringLength(50,MinimumLength =2)]
        public string Title { get; set; }//İs adresim baslik
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Details { get; set; } //adres detayi
        //UserId buaraya geri donecegiz.
        public int NeighbourhoodId { get; set; }//FK
        public bool IsDefaultAddress { get; set; }
        [ForeignKey("Neighbourhood")]
        public virtual Neighbourhood NeighbourhoodFK { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
