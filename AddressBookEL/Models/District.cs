using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookEL.Models
{
    [Table("Districts")]
    public class District:BaseNumeric
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        public int CityId { get; set; }
        [ForeignKey("CityId")] //CityId'ye yazdigim int degerinin City tablosunda karsiligi olmak zorunda.
        public virtual City City { get; set; } //CityId propertysi ForeignKey olacagi icin burada City Tablosuyla ilski kuruldu.
        //DİPNOT: Iliskiler burada kurulacagi gibi MYCONTEXT classi icindeki OnModelCreating metodu ezilerek(override) kurulabilir.
    }
}
