using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Id")]
        public override string Id { get; set; } = "";

        [Display(Name = "Email")]
        public override string Email { get; set; } = "";

        [Display(Name = "Jméno")]
        public string Jmeno { get; set; } = "";

        [Display(Name = "Příjmení")]
        public string Prijmeni { get; set; } = "";
    }
}


//https://www.learnentityframeworkcore.com/relationships