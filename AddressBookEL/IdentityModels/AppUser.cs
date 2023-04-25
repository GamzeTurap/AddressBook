using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookEL.IdentityModels
{
    public class AppUser:IdentityUser
    {
        //IdentityUser classinin icindeki propertylerden farkli eklemek istedigin ozellikler var ise IdentityUser classindan kalitim alarak tabloyu genisletebiliriz.
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(50,MinimumLength =2)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate{ get; set; }
        // public string Gender { get; set; } e ticaret sitesinde gerekebilir.
        public bool IsPassive { get; set; }
        [Required]
        [StringLength(10,MinimumLength =10)]
        [RegularExpression("^[0-9]*", ErrorMessage ="Telefon rakamlardan olusmalidir.")]
        public override string PhoneNumber { get; set; }
    }
}




