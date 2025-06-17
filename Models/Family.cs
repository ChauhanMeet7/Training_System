using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class Family
    {
        [Key]
        public int Id { get; set; }
        public int TraineeId { get; set; }


        [ForeignKey("TraineeId")]

        public Trainee? Trainee { get; set; }

        public string FatherName { get; set; }
        public int FatherAge { get; set; }
        public string FatherQulification { get; set; }
        public string FatherProfession { get; set; }
        public int FatherIncome { get; set; }
        public string MotherName { get; set; }
        public int MotherAge { get; set; }
        public string MotherQulification { get; set; }
        public string MotherProfession { get; set; }
        public int MotherIncome { get; set; }
        public string GuardianName { get; set; }
        public int GuardianAge { get; set; }
        public string GuardianQulification { get; set; }
        public string GuardianProfession { get; set; }
        public int GuardianIncome { get; set; }
        public string SisterName { get; set; }
        public int SisterAge { get; set; }
        public string SisterQulification { get; set; }
        public string SisterProfession { get; set; }
        public int SisterIncome { get; set; }
        public string BrotherName { get; set; }
        public int BrotherAge { get; set; }
        public string BrotherQulification { get; set; }
        public string BrotherProgession { get; set; }
        public int BrotherIncome { get; set; }
    }
}
