using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Gnfc Student { get; set; }

        //[Required]
        //public DateTime Date { get; set; }

        //[Required]
        //public bool IsPresent { get; set; }
        [Required]
        public DateTime Date { get; set; } // Admin can set a custom date

        [Required]
        public string Status { get; set; } // Present, Absent, Late

        public TimeSpan TimeRecorded { get; set; } = DateTime.Now.TimeOfDay;








        //[Required]
        //public DateTime Date { get; set; } = DateTime.Now; // Auto-timestamp when submitted

        //[Required]
        //public string Status { get; set; } // "Present", "Absent", or "Late"

        //public TimeSpan TimeRecorded { get; set; } = DateTime.Now.TimeOfDay;
    }

}

