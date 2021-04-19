using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace foodisgood.Models
{
    public class OrdersViewModel
    {
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public IEnumerable<Order> Orders { get; set; }

    }
}