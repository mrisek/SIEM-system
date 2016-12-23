using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Configuration
    {
        // Services
        [Required(ErrorMessage = "Unesite interval u minutama.")]
        [Display(Name = "IntervalMinutes")]
        public string IntervalMinutes { get; set; } // u bazu ide samo kao Name !!!

        [Required(ErrorMessage = "Unesite vrijeme.")]
        [Display(Name = "ScheduledTime")]
        public string ScheduledTime { get; set; }

        [Required(ErrorMessage = "Unesite naziv servisa.")]
        [Display(Name = "ServiceName")]
        public string ServiceName { get; set; }


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


        // Machines

        //TODO: dodati konfiguraciju postavki prema ID-ju za slučaj da više servera ima jednake nazive
        [Required(ErrorMessage = "Unesite naziv uređaja.")]
        [Display(Name = "MachineName")]
        public string MachineName { get; set; }

        [Required(ErrorMessage = "Unesite postavke događaja.")]
        [Display(Name = "EventSettings")]
        public string EventSettings { get; set; }

        [Required(ErrorMessage = "Unesite postavke pravila.")]
        [Display(Name = "RuleSettings")]
        public string RuleSettings { get; set; }

        [Required(ErrorMessage = "Unesite postavke servisa.")]
        [Display(Name = "ServiceSettings")]
        public string ServiceSettings { get; set; }


        // Events
        [Required(ErrorMessage = "Unesite spremnik.")]
        [Display(Name = "Container")]
        public string Container { get; set; }

        [Required(ErrorMessage = "Unesite poruku pogreške.")]
        [Display(Name = "EnableRaisingEvents")]
        public string EnableRaisingEvents { get; set; }

        [Required(ErrorMessage = "Unesite unose.")]
        [Display(Name = "Entries")]
        public string Entries { get; set; }

        [Required(ErrorMessage = "Unesite EventsLog.")]
        [Display(Name = "EventsLog")]
        public string EventsLog { get; set; }

        [Required(ErrorMessage = "Unesite LogDisplayName.")]
        [Display(Name = "LogDisplayName")]
        public string LogDisplayName { get; set; }

        [Required(ErrorMessage = "Unesite naziv mašine.")]
        [Display(Name = "MachineName")]
        public string MachineNameEvents { get; set; } // samo MachineName

        [Required(ErrorMessage = "Unesite maksimalni broj KB.")]
        [Display(Name = "MaximumKilobytes")]
        public string MaximumKilobytes { get; set; }

        [Required(ErrorMessage = "Unesite minimalni broj dana pohrane.")]
        [Display(Name = "MinimumRetentionDays")]
        public string MinimumRetentionDays { get; set; }

        [Required(ErrorMessage = "Unesite OverflowAction.")]
        [Display(Name = "OverflowAction")]
        public string OverflowAction { get; set; }

        [Required(ErrorMessage = "Unesite Site.")]
        [Display(Name = "Site")]
        public string Site { get; set; }

        [Required(ErrorMessage = "Unesite izvor.")]
        [Display(Name = "Source")]
        public string Source { get; set; }

        [Required(ErrorMessage = "Unesite sinkronizacijski objekt.")]
        [Display(Name = "SynchronizingObject")]
        public string SynchronizingObject { get; set; }


        // Logs
        [Required(ErrorMessage = "Unesite EventID.")]
        [Display(Name = "EventID")]
        public string EventID { get; set; }

        [Required(ErrorMessage = "Unesite InstanceID.")]
        [Display(Name = "InstanceID")]
        public string InstanceID { get; set; }

        [Required(ErrorMessage = "Unesite naziv uređaja.")]
        [Display(Name = "MachineName")]
        public string MachineNameLogs { get; set; } // samo MachineName

        [Required(ErrorMessage = "Unesite zamjenske stringove.")]
        [Display(Name = "ReplacementStrings")]
        public string ReplacementStrings { get; set; }

        [Required(ErrorMessage = "Unesite Site.")]
        [Display(Name = "Site")]
        public string SiteLogs { get; set; } // samo Site

        [Required(ErrorMessage = "Unesite izvor.")]
        [Display(Name = "Source")]
        public string SourceLogs { get; set; } // samo Source

        [Required(ErrorMessage = "Unesite vrijeme generiranja.")]
        [Display(Name = "TimeGenerated")]
        public string TimeGenerated { get; set; }

        [Required(ErrorMessage = "Unesite vrijeme zapisa.")]
        [Display(Name = "TimeWritten")]
        public string TimeWritten { get; set; }

        [Required(ErrorMessage = "Unesite korisničko ime.")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Unesite poruku.")]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Unesite podatke.")]
        [Display(Name = "Data")]
        public string Data { get; set; }

        [Required(ErrorMessage = "Unesite kategoriju.")]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Unesite broj kategorije.")]
        [Display(Name = "CategoryNumber")]
        public string CategoryNumber { get; set; }

        [Required(ErrorMessage = "Unesite spremnik.")]
        [Display(Name = "Container")]
        public string ContainerLogs { get; set; } // samo Container

        [Required(ErrorMessage = "Unesite tip unosa.")]
        [Display(Name = "EntryType")]
        public string EntryType { get; set; }

    }
}