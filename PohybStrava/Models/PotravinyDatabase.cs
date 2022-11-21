using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class PotravinyDatabase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int PotravinaDatabaseId { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; } = "";

        [Display(Name = "Potravina")]
        public string PotravinaDatabase { get; set; } = "";

        [Display(Name = "Jednotka")]
        public string Jednotka { get; set; } = "";

        [Display(Name = "Energetická hodnota (kcal/jednotka) ")]
        public double EnergieDatabase { get; set; }
    }
}
