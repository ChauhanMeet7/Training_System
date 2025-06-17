using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Trainee
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "GNFC ID ")]

        public string VTRId { get; set; }
        public int TrainingYear { get; set; }
    }
}
