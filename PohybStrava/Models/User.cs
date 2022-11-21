using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PohybStrava.Models;
using Microsoft.EntityFrameworkCore;

namespace PohybStrava.Models
{
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int UserId { get; set; }

        [Display(Name = "Datum")]
        public DateTime DatumUser { get; set; }

        [Display(Name = "Jméno")]
        public string Jmeno { get; set; } = "";

        [Display(Name = "Příjmení")]
        public string Prijmeni { get; set; } = "";

        [Display(Name = "E-mail")]
        public string Email { get; set; } = "";

        [Display(Name = "Pohlaví (muž/žena)")]
        public string Pohlavi { get; set; } = "";

        [Display(Name = "Věk")]
        public double Vek { get; set; }

        [Display(Name = "Váha")]
        public double Vaha { get; set; }

        [Display(Name = "Výška (cm)")]
        public double Vyska { get; set; }

        private double bmi;
        private double bmr;

        [Display(Name = "BMI")]
        public double BMI
        {
            get { return Math.Round(Vaha / ((Vyska / 100) * (Vyska / 100)), 2); }
            set { bmi = value; }
        }

        [Display(Name = "Bazální metabolismus (kcal)")]
        public double BMR
        {
            get
            {
                {
                    if (Pohlavi == "žena")
                    { return Math.Round((655.0955 + (9.5634 * Vaha) + (1.8496 * Vyska) - (4.6756 * Vek)), 0); }

                    else
                    { return Math.Round((66.473 + (13.7516 * Vaha) + (5.0033 * Vyska) - (6.755 * Vek)), 0); }
                };
            }
            set { bmr = value; }
        }

        public User()
        {
            Diets = new HashSet<Diet>();
            Activities = new HashSet<Activities>();
            PrijemVydejEnergie = new HashSet<PrijemVydejEnergie>();
        }

        public int den;
        public int mesic;
        public int rok;

        public int Den
        {
            get { return int.Parse(DatumUser.ToString("dd")); }
            set { mesic = value; }
        }

        [Display(Name = "Měsíc")]
        public int Mesic
        {
            get { return int.Parse(DatumUser.ToString("MM")); }
            set { mesic = value; }
        }

        public int Rok
        {
            get { return int.Parse(DatumUser.ToString("yyyy")); }
            set { rok = value; }
        }

        public ICollection<Diet> Diets { get; set; }
        public ICollection<Activities> Activities { get; set; }
        public ICollection<PrijemVydejEnergie> PrijemVydejEnergie { get; set; }
        public double VahaPrumer { get; set; }
        public double BMIPrumer { get; set; }


    }
}




