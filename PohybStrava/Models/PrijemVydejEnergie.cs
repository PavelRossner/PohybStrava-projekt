using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class PrijemVydejEnergie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int PrijemVydejId { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; } = "";

        [Display(Name = "Datum")]
        public DateTime DatumDiet { get; set; }

        [Display(Name = "Datum")]
        public DateTime DatumActivities { get; set; }

        [Display(Name = "Energie přijatá")]
        public int SoucetEnergieDiet { get; set; }

        [Display(Name = "Energie vydaná")]
        public int SoucetEnergieActivities { get; set; }

        [Display(Name = "Bazální metabolismus")]
        public double BMR { get; set; }

        [Display(Name = "Energetická bilance")]
        public double EnergetickaBilance
        {
            get { return SoucetEnergieDiet - (SoucetEnergieActivities + BMR); }
            set { energetickaBilance = value; }
        }

        public double celkemEnergie;
        public double energetickaBilance;
        public int den;
        public int mesic;
        public int rok;

        public int Den
        {
            get { return int.Parse(DatumDiet.ToString("dd")); }
            set { mesic = value; }
        }

        [Display(Name = "Měsíc")]
        public int Mesic
        {
            get { return int.Parse(DatumDiet.ToString("MM")); }
            set { mesic = value; }
        }

        public int Rok
        {
            get { return int.Parse(DatumDiet.ToString("yyyy")); }
            set { rok = value; }
        }

    }
}
