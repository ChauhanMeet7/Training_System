using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Posting
    {

        [Key]
        public int Id { get; set; }
        public int TraineeId { get; set; }


        [ForeignKey("TraineeId")]

        public Trainee? Trainee { get; set; }

        
        [Required]
        public int PlantId { get; set; }


        [ForeignKey("PlantId")]

        public Plant? Plant { get; set; }
       

    }
}
