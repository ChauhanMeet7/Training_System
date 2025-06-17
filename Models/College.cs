using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class College
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "College Name")]
        public string CollegeName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }

    }
}
