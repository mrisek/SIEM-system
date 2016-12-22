using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Configuration
    {
        // Rules
        [Required(ErrorMessage = "Unesite naziv pravila.")]
        [Display(Name = "RuleName")]
        public string RuleName { get; set; } // u bazu ide samo kao Name !!!

        [Required(ErrorMessage = "Unesite minimalnu razinu.")]
        [Display(Name = "MinLevel")]
        public string MinLevel { get; set; }

        [Required(ErrorMessage = "Unesite odredište.")]
        [Display(Name = "WriteTo")]
        public string WriteTo { get; set; }


        // Targets
        [Required(ErrorMessage = "Unesite tip.")]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Unesite naziv.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Unesite adresu.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Unesite naziv datoteke.")]
        [Display(Name = "FileName")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Popunite polje.")]
        [Display(Name = "Layout")]
        public string Layout { get; set; }

        [Required(ErrorMessage = "Unesite naziv odredišta.")]
        [Display(Name = "TargetName")]
        public string TargetName { get; set; }
    }
}