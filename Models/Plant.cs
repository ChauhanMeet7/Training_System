using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Plant
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Plant Name")]


        public string Name { get; set; }
    }
}
