using System.ComponentModel.DataAnnotations;

namespace foodisgood.Models
{
    public class Rewiew
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "Rewiew message")]
        public string Text { get; set; }

    }
}