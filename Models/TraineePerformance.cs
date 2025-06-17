using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class TraineePerformance
    {
        [Key]
        public int Id { get; set; }

        public string VTRId { get; set; } // Unique Trainee ID
        public string TraineeName { get; set; }
        public string CollegeName { get; set; } // Changed from CourseName to CollegeName
        public string Department { get; set; }
        public DateTime TrainingStartDate { get; set; }
        public DateTime TrainingEndDate { get; set; }
        public double AttendancePercentage { get; set; } // Added field
        public string PerformanceRemarks { get; set; }
    }
}
