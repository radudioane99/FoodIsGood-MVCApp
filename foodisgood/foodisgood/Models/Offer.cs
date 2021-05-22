 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace foodisgood.Models
{
    public class Offer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Offer Name")]
        public string Name { get; set; }

        [Required]
        public float PriceUnit { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public bool Expired { get; set; }

        public string Description { get; set; }

        public virtual Product Product { get; set; }
        [Display(Name = "Product")]
        public int ProductID { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string UserID { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}