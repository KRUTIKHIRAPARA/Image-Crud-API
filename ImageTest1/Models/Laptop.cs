using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageTest1.Models
{
    public class Laptop
    {
        [Key]
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }

        [NotMapped]
        [Display(Name ="Choose Image")]
        public IFormFile ImagePath { get; set; }

    }
}
