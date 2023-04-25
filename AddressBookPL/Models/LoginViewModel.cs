using System.ComponentModel.DataAnnotations;

namespace AddressBookPL.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserNameorEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
