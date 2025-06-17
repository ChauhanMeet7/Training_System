using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Gnfc
    {

        [Key]
        public int Id { get; set; }
        public int TraineeId { get; set; }


        [ForeignKey("TraineeId")]

        public Trainee? Trainee { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public int DepartmentId { get; set; }


        [ForeignKey("DepartmentId")]

        public Department? Department { get; set; }
        public string Email { get; set; }
        public string Birth_Date { get; set; }
        public string PhoneNumber { get; set; }
        public int CollegeId { get; set; }


        [ForeignKey("CollegeId")]

        public College? College { get; set; }
        public int AcadamicYear { get; set; }
        public int Semester { get; set; }

    }
}
