using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class PersonalDetail
    {
        [Key]
        public int Id { get; set; }
        public int TraineeId { get; set; }


        [ForeignKey("TraineeId")]

        public Trainee? Trainee { get; set; }
        public string Religion { get; set; }
        [Required]
        public string Gender { get; set; }
        public string Blood_Group { get; set; }
        public string PhysicallyHandicapped { get; set; }
        public string MaritalStatus { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string LocalAddress { get; set; }
        public string PermanentAddress { get; set; }

    }
}
