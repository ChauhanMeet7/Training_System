using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Academic
    {

        [Key]
        public int Id { get; set; }
        public int TraineeId { get; set; }


        [ForeignKey("TraineeId")]

        public Trainee? Trainee { get; set; }

        public string Examination { get; set; }
        public string Board { get; set; }
        public string Passing_year { get; set; }
       
        public string School { get; set; }
        public int Percentage { get; set; }
        



    }
}
