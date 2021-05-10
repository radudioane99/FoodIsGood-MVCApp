using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.AspNet.Identity.EntityFramework;

namespace foodisgood.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string BuyerID { get; set; }

        public virtual ApplicationUser BuyerUser { get; set; }

        [Required]
        public int OfferID { get; set; }

        public virtual Offer Offer { get; set; }

        [Required]
        [Display(Name = "Desired Quantity")]
        public float DesiredQuantity { get; set; }

        public bool Accepted { get; set; }
    }
}