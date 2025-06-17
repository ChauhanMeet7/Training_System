using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class TrainingCompletionReport
    {
        [Key]
        public int ReportID { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TrainingStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TrainingEndDate { get; set; }

        [Required]
        public string CompletionStatus { get; set; }

        [Range(0, 100)]
        public int WrittenTestScore { get; set; }

        public string PerformanceRemarks { get; set; }
    }
}
