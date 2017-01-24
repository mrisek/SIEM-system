using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class LogsModel
    {
        [Required(ErrorMessage = "Unesite log.")]
        [Display(Name = "Index")]
        public string Index { get; set; }

        [Required(ErrorMessage = "Unesite naziv mašine.")]
        [Display(Name = "MachineName")]
        public string MachineName { get; set; }

        [Required(ErrorMessage = "Unesite izvor.")]
        [Display(Name = "Source")]
        public string Source { get; set; }

        [Required(ErrorMessage = "Unesite vrijeme generiranja.")]
        [Display(Name = "TimeGenerated")]
        public string TimeGenerated { get; set; }

        [Required(ErrorMessage = "Unesite poruku.")]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Unesite tip unosa.")]
        [Display(Name = "EntryType")]
        public string EntryType { get; set; }
    }
}