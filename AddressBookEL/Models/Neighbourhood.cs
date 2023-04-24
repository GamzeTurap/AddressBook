using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookEL.Models
{
    [Table("Neighbourhoods")]
    public class Neighbourhood:BaseNumeric
    {
        //Eger bu tablonun Pk'si string olsaydi BAseNumericten kalitim alamazdi.! Bu senaryoda;
        //1)BaseNonNumeric classi olusturabilir
        //2)KAlitim almadan bu classin ici direkt olarak prop tanimlanabilir.
        //--->public string Id{get;set;}
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string PlateCode { get; set; }
        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
    }
}
