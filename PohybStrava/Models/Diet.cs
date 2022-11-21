using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class Diet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]

        public int DietId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; } = "";

        [Display(Name = "Datum")]
        public DateTime DatumDiet { get; set; }

        [Display(Name = "Potravina")]
        public string Potravina { get; set; } = "";

        [Display(Name = "Energie (kcal)")]
        public double Energie { get; set; }

        [Display(Name = "Množství")]
        public double Mnozstvi { get; set; }

        public int SoucetEnergieDiet { get; set; }

        public int den;
        public int mesic;
        public int rok;
        public double celkemEnergie;

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

        [Display(Name = "Energie celkem (kcal)")]
        public double Celkem
        {
            get { return (Energie * Mnozstvi); }
            set { celkemEnergie = value; }
        }

    }
}
