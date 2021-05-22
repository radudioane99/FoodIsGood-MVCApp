using System;
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
        [Display(Name = "Review message")]
        public string Text { get; set; }

        public DateTime date { get; set; }

        public int note { get; set; }

    }
}