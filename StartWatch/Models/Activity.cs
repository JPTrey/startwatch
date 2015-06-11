using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StartWatch.Models
{
    public class Activity
    {
        public int Id { get; set; }             // hidden
        public string UserId { get; set; }         // hidden
        [Required]
        [Display(Name="Daily Quota (mins)")]
        public int Quota { get; set; }          // UNIX timestamp
        [Display(Name = "Done Today")]
        public int ProgressToday { get; set; }  // UNIX timestamp, hidden
        [Display(Name = "This Week")]
        public int ProgressWeek { get; set; }   // UNIX timestamp, hidden
        [Display(Name = "Total")]
        public int ProgressTotal { get; set; }   // UNIX timestamp, hidden
        [Display(Name = "Logged Since")]
        public DateTime CreationDate { get; set; }   // hidden
        public bool Required { get; set; }
        [Required]
        [Display(Name="Activity Name")]
        public string Name { get; set; }
        public string[] Days { get; set; }  
        public int SessionCount { get; set; }       // hidden
    }
}