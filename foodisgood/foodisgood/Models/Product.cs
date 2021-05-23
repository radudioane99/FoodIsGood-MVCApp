using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace foodisgood.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}