using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foodisgood.Models
{
    public class Offer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Price")]
        public float PriceUnit { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public float Quantity { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreateTime { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndTime { get; set; }

        [Required]
        public bool Expired { get; set; }

        public string Description { get; set; }

        [Display(Name = "Product")]
        public int ProductID { get; set; }

        public virtual Product Product { get; set; }

        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public string StarsAverage { get; set; }

    }
}