using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class Activities
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int ActivitiesId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; } = "";

        [Display(Name = "Datum")]
        public DateTime DatumActivities { get; set; }

        [Display(Name = "Název trasy")]
        public string Trasa { get; set; } = "";

        [Display(Name = "Vzdálenost (km)")]
        public double Vzdalenost { get; set; }

        [Display(Name = "Převýšení (m)")]
        public double Prevyseni { get; set; }

        [Display(Name = "Čas (hod., min.)")]
        public string Cas { get; set; } = "";

        [Display(Name = "Tempo (min/km)")]
        public double Tempo { get; set; }

        [Display(Name = "Energie (kcal)")]
        public double Energie { get; set; }

        public int den;
        public int mesic;
        public int rok;

        [Display(Name = "Den")]
        public int Den
        {
            get { return int.Parse(DatumActivities.ToString("dd")); }
            set { den = value; }
        }

        [Display(Name = "Měsíc")]
        public int Mesic
        {
            get { return int.Parse(DatumActivities.ToString("MM")); }
            set { mesic = value; }
        }

        public int Rok
        {
            get { return int.Parse(DatumActivities.ToString("yyyy")); }
            set { rok = value; }
        }

        public int SoucetVzdalenost { get; set; }
        public int SoucetPrevyseni { get; set; }
        public int SoucetEnergieActivities { get; set; }

    }

}
