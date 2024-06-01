using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crud.Models
{
    public class Todo
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Title { get; set;}
        public DateTime CreatedDate { get; set; }
        public bool Done { get; set; }
        public int UserId { get; set; }
    }
}
